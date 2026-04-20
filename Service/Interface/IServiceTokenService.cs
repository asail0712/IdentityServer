using AetherCore.Service;
using Common.DTO.ServiceToken;
using Microsoft.AspNetCore.Mvc;

namespace Service.Interface
{
    public interface IServiceTokenService : IService<ServiceTokenRequest, ServiceTokenResponse>
    {
        Task<ServiceTokenResponse> RequestServiceToken([FromBody] ServiceTokenRequest request);
    }
}
