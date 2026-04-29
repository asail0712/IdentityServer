using AetherCore.Auth.PasswordLogin;
using AetherCore.Controller;
using AetherCore.Utility.Attributes;
using Common.DTO.ServiceToken;
using Common.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interface;
using System.Security.Claims;

namespace IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [CrudSummary("服務器Token")]
    //[CrudAuthorize("AdminJwt")]
    public class ServiceTokenController : GenericController<ServiceTokenRequest, ServiceTokenResponse, IServiceTokenService>
    {
        public ServiceTokenController(IServiceTokenService service)
            : base(service, CrudOperation.None)
        {

        }

        [AllowAnonymous]
        [HttpPost("RequestServiceToken")]
        [CommonSummary("申請Token")]
        [SwaggerApi("Test")]
        public async Task<IActionResult> RequestServiceToken([FromBody] ServiceTokenRequest request)
        {
            if (request == null)
                return BadRequest("Request 不可為空");

            if (string.IsNullOrWhiteSpace(request.ServiceId)
                || string.IsNullOrWhiteSpace(request.RegistryId)
                || string.IsNullOrWhiteSpace(request.RegistrySecret))
                return BadRequest("參數錯誤");

            var bResult = await _service.RequestServiceToken(request);

            return Ok(bResult);
        }
    }
}
