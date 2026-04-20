using AetherCore.DataAccess;
using AetherCore.Utility.Attributes;
using AutoMapper;
using Common.DTO.Login;
using DataAccess.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class LoginDataAccess : MongoEntityDataAccess<LoginEntity, LoginDocument>, ILoginDataAccess
    {
        public LoginDataAccess(IMapper mapper)
            : base(mapper)
        {
        }
    }
}
