using AetherCore.Auth.PasswordLogin;
using AetherCore.Controller;
using AetherCore.Utility.Attributes;
using Common.DTO.ServiceRegistry;
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
    [CrudSummary("服務器註冊資料")]
    //[CrudAuthorize("AdminJwt")]
    public class ServiceRegistryController : GenericController<ServiceRegistryRequest, ServiceRegistryResponse, IServiceRegistryService>
    {
        public ServiceRegistryController(IServiceRegistryService service)
            : base(service, CrudOperation.None)
        {

        }
    }
}
