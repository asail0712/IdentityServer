using AetherCore.Auth.PasswordLogin;
using AetherCore.Service;
using Common.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace Service.Interface
{
    public interface IUserService : IService<UserRequest, UserResponse>
    {
        Task<bool> CreateUser([FromBody] PasswordLoginRequest request);
    }
}
