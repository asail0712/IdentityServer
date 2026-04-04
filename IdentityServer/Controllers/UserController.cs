using AetherCore.Auth.PasswordLogin;
using AetherCore.Controller;
using AetherCore.Utility.Attributes;
using Common.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interface;
using System.Security.Claims;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [CrudSummary("使用者")]
    //[CrudAuthorize("AdminJwt")]
    public class UserController : GenericController<UserRequest, UserResponse, IUserService>
    {
        public UserController(IUserService service)
            : base(service, CrudOperation.All & ~CrudOperation.Create)
        {

        }

        [HttpGet("GetProfile")]
        [CommonSummary("取得使用者資訊")]
        [Authorize(AuthenticationSchemes = "AppJwt")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var bResult = await _service.GetAsync(userId);

            return Ok(bResult);
        }

        [HttpPost("CreateUser")]
        [CommonSummary("創建新帳號")]
        //[Authorize(AuthenticationSchemes = "AdminJwt")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] PasswordLoginRequest request)
        {
            bool success = await _service.CreateUser(request);

            return Ok(success);
        }
    }
}
