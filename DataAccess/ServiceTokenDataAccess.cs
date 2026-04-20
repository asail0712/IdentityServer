using AetherCore.DataAccess;
using AetherCore.Utility.Attributes;
using AutoMapper;
using Common.DTO.ServiceToken;
using DataAccess.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class ServiceTokenDataAccess : MongoEntityDataAccess<ServiceTokenEntity, ServiceTokenDocument>, IServiceTokenDataAccess
    {
        public ServiceTokenDataAccess(IMapper mapper)
            : base(mapper)
        {
        }
    }
}
