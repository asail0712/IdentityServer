using AetherCore.DataAccess;
using AetherCore.Utility.Attributes;
using AutoMapper;
using Common.DTO.ServiceRegistry;
using DataAccess.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class ServiceRegistryDataAccess : MongoEntityDataAccess<ServiceRegistryEntity, ServiceRegistryDocument>, IServiceRegistryDataAccess
    {
        public ServiceRegistryDataAccess(IMapper mapper)
            : base(mapper)
        {
            EnsureIndexCreated("RegistryId");
        }
    }
}
