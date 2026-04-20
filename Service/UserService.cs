using AetherCore.Auth.PasswordLogin;
using AetherCore.Exceptions;
using AetherCore.Service;
using AetherCore.Utility;
using AetherCore.Utility.Attributes;
using AutoMapper;
using Common.DTO.Login;
using Common.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Common;
using Repository.Interface;
using Service.Interface;

namespace Service
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class UserService : GenericService<UserEntity, UserRequest, UserResponse, IUserRepository>, IUserService
    {
        private readonly ILoginRepository _authRepo;

        public UserService(IUserRepository repo, IMapper mapper, ILoginRepository authRepo) 
            : base(repo, mapper)
        {
            _authRepo = authRepo;
        }

        public async Task<bool> CreateUser([FromBody] PasswordLoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Account) || string.IsNullOrWhiteSpace(request.Password))
                throw new InvalidCredentialsException("Account 和 Password 不可為空");

            // 1. 查 Auth 是否存在
            var existingAuth = (await _authRepo.QueryAsync(x =>
                x.IsEnabled &&
                x.Provider == ProviderDefine.Password &&
                x.Account == request.Account))
                ?.FirstOrDefault();
            if (existingAuth != null)
                throw new InvalidCredentialsException("帳號已存在");

            // 2. 建立 User + Auth
            var now     = DateTime.UtcNow;
            var user    = new UserEntity
            {
                DisplayName = request.Account,
                CreatedAt   = now,
                UpdatedAt   = now
            };
            user        = await _repository.InsertAsync(user);
            var auth    = new LoginEntity
            {
                UserId          = user.Id,
                Provider        = ProviderDefine.Password,
                Account         = request.Account,
                PasswordHash    = Utils.ComputeSha256Hash(request.Password, CommonDefine.SHA256_SALT),
                CreatedAt       = now,
                UpdatedAt       = now
            };
            await _authRepo.InsertAsync(auth);
            return true;
        }
    }
}
