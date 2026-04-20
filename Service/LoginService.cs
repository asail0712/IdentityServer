using AetherCore.Auth.LineLogin;
using AetherCore.Auth.PasswordLogin;
using AetherCore.DTO.PasswordLogin;
using AetherCore.Exceptions;
using AetherCore.Module.Token.Interface;
using AetherCore.Service;
using AetherCore.Utility;
using AetherCore.Utility.Attributes;
using AetherCore.Utility.Auth.Line;
using AutoMapper;
using Common;
using Common.DTO.Login;
using Common.DTO.Settings;
using Common.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repository;
using Repository.Interface;
using Service.Interface;
using System.Net.Http;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class LoginService : GenericService<LoginEntity, LoginRequest, LoginResponse, ILoginRepository>, ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;
        private readonly LineLoginSettings _lineSettings;

        public LoginService(ILoginRepository repo,
                            IMapper mapper,
                            IUserRepository userRepository,
                            IHttpClientFactory httpClientFactory,
                            ITokenServiceFactory tokenServiceFactory,
                            IOptions<LineLoginSettings> opt)
            : base(repo, mapper)
        {
            _userRepository     = userRepository;
            _httpClientFactory  = httpClientFactory;
            _lineSettings       = opt.Value;
            _tokenService       = tokenServiceFactory.Create("AppJwt");
        }

        public async Task<LineLoginResponse> LineLogin(LineLoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.LineAccessToken))
                throw new InvalidCredentialsException("LineAccessToken 不可為空");

            // 1. 驗證 access token
            var verifyResult = await LineApiHelper.VerifyAccessTokenAsync(_httpClientFactory.CreateClient(), request.LineAccessToken);

            if (string.IsNullOrWhiteSpace(verifyResult.ClientId) ||
               verifyResult.ClientId != _lineSettings.ChannelId)
            {
                throw new InvalidCredentialsException("LINE access token 的 client_id 不符合目前系統設定");
            }

            if (verifyResult.ExpiresIn <= 0)
            {
                throw new InvalidCredentialsException("LINE access token 已過期");
            }

            // 2. 取得 profile
            var profile = await LineApiHelper.GetProfileAsync(_httpClientFactory.CreateClient(), request.LineAccessToken);

            if (string.IsNullOrWhiteSpace(profile.UserId))
                throw new InvalidCredentialsException("無法從 LINE profile 取得 userId");

            // 3. 查 Auth 是否存在
            var auth = (await _repository.QueryAsync(x =>
                x.IsEnabled &&
                x.Provider == ProviderDefine.Line &&
                x.ProviderUserId == profile.UserId))
                ?.FirstOrDefault();

            // 4. 第一次登入：建立 User + Auth
            if (auth == null)
            {
                var now     = DateTime.UtcNow;
                var user    = new UserEntity
                {
                    DisplayName = profile.DisplayName,
                    AvatarUrl   = profile.PictureUrl,
                    CreatedAt   = now,
                    UpdatedAt   = now
                };

                user = await _userRepository.InsertAsync(user);
                auth = new LoginEntity
                {
                    UserId          = user.Id,
                    Provider        = ProviderDefine.Line,
                    ProviderUserId  = profile.UserId,
                    CreatedAt       = now,
                    UpdatedAt       = now
                };

                auth = await _repository.InsertAsync(auth);
            }

            // 5. 加上Claim
            var extraClaims = new List<Claim>
            {
                new Claim(ClaimLoginType.AvatarUrl, profile.PictureUrl ?? string.Empty),
                new Claim(ClaimLoginType.Provider, ProviderDefine.Line)
            };

            // 6. 回傳登入結果
            return new LineLoginResponse
            {
                AccessToken = _tokenService.GenerateToken(auth.UserId, profile.DisplayName, TimeSpan.FromDays(7), extraClaims)
            };
        }

        public async Task<PasswordLoginResponse> PasswordLogin([FromBody] PasswordLoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Account) || string.IsNullOrWhiteSpace(request.Password))
                throw new InvalidCredentialsException("帳密不得為空");

            // 1. 查 Auth 是否存在
            var auth = (await _repository.QueryAsync(x =>
                x.IsEnabled &&
                x.Provider == ProviderDefine.Password &&
                x.Account == request.Account))
                ?.FirstOrDefault();
            if (auth == null)
                throw new InvalidCredentialsException("帳號不存在");

            // 2. 驗證密碼
            var inputHash = Utils.ComputeSha256Hash(request.Password, CommonDefine.SHA256_SALT);
            if (!string.Equals(auth.PasswordHash, inputHash, StringComparison.Ordinal))
                throw new InvalidCredentialsException("密碼有誤");

            // 3. 加上Claim
            var extraClaims = new List<Claim>
            {
                new Claim(ClaimLoginType.AvatarUrl, string.Empty),
                new Claim(ClaimLoginType.Provider, ProviderDefine.Password)
            };

            // 4. 回傳登入結果
            return new PasswordLoginResponse
            {
                AccessToken = _tokenService.GenerateToken(auth.UserId, request.Account, TimeSpan.FromDays(7), extraClaims)
            };
        }
    }
}
