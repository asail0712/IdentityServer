using AetherCore.Auth.LineLogin;
using AetherCore.Auth.PasswordLogin;
using AetherCore.Controller;
using AetherCore.Utility.Attributes;
using Common.DTO.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [CrudSummary("登入資訊")]
    [CrudAuthorize("AppJwt")]
    public class LoginController : GenericController<LoginRequest, LoginResponse, ILoginService>
    {
        public LoginController(ILoginService service)
            : base(service, CrudOperation.None)
        {

        }

        [AllowAnonymous]
        [HttpPost("LineLogin")]
        [CommonSummary("Line登入")]
        public async Task<IActionResult> LineLogin([FromBody] LineLoginRequest request)
        {
            var bResult = await _service.LineLogin(request);

            return Ok(bResult);
        }

        [AllowAnonymous]
        [HttpPost("PasswordLogin")]
        [CommonSummary("密碼登入")]
        public async Task<IActionResult> PasswordLogin([FromBody] PasswordLoginRequest request)
        {
            var bResult = await _service.PasswordLogin(request);

            return Ok(bResult);
        }
    }
}
