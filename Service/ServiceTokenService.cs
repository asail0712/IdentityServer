using AetherCore.Exceptions;
using AetherCore.Module.Token.Interface;
using AetherCore.Service;
using AetherCore.Utility.Attributes;
using AutoMapper;
using Common;
using Common.DTO.ServiceRegistry;
using Common.DTO.ServiceToken;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interface;
using Service.Interface;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class ServiceTokenService : GenericService<ServiceTokenEntity, ServiceTokenRequest, ServiceTokenResponse, IServiceTokenRepository>, IServiceTokenService
    {
        private readonly IServiceRegistryRepository _registryRepo;
        private readonly ITokenService _tokenService;

        public ServiceTokenService(IServiceTokenRepository repo
                                    , IServiceRegistryRepository registryRepoe
                                    , ITokenServiceFactory tokenServiceFactory
                                    , IMapper mapper) 
            : base(repo, mapper)
        {
            _registryRepo = registryRepoe;
            _tokenService = tokenServiceFactory.Create("ServiceJwt");
        }

        public async Task<ServiceTokenResponse> RequestServiceToken(ServiceTokenRequest request)
        {
            // 查詢是某有這組註冊紀錄
            var entity              = await _registryRepo.GetAsync(request.RegistryId);
            var bResult             = request.RegistrySecret == entity.RegistrySecret;

            // 將此筆申請紀錄起來
            var tokenEntity         = _mapper.Map<ServiceTokenEntity>(request);
            tokenEntity.Success     = bResult;
            var resultEntity        = await _repository.InsertAsync(tokenEntity);

            // 完善這個response
            var response            = _mapper.Map<ServiceTokenResponse>(resultEntity);
            response.ServiceToken   = bResult ? GenerateServiceToken(entity, request.ServiceId) :"";

            return response;
        }

        private string GenerateServiceToken(ServiceRegistryEntity entity, string serviceId)
        {
            var extraClaims = new List<Claim>
            {
                new Claim(ClaimLoginType.EndPoint, entity.Endpoint),
                new Claim(ClaimLoginType.RegistryId, entity.RegistryId),
                new Claim(ClaimLoginType.ServiceId, serviceId)
            };

            return _tokenService.GenerateToken(serviceId, entity.RegistryId, TimeSpan.FromMinutes(entity.ExpiresInMin), extraClaims);
        }
    }
}
