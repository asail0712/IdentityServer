using AetherCore.Auth.LineLogin;
using AetherCore.Auth.PasswordLogin;
using AetherCore.DTO.PasswordLogin;
using AetherCore.Service;
using Common.DTO.Login;
using Microsoft.AspNetCore.Mvc;

namespace Service.Interface
{
    public interface ILoginService : IService<LoginRequest, LoginResponse>
    {
        Task<LineLoginResponse> LineLogin([FromBody] LineLoginRequest request);
        Task<PasswordLoginResponse> PasswordLogin([FromBody] PasswordLoginRequest request);
    }
}
