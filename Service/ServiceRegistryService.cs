using AetherCore.Service;
using AetherCore.Utility.Attributes;
using AutoMapper;
using Common.DTO.ServiceRegistry;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interface;
using Service.Interface;

namespace Service
{
    [AutoInject(ServiceLifetime.Scoped)]
    public class ServiceRegistryService : GenericService<ServiceRegistryEntity, ServiceRegistryRequest, ServiceRegistryResponse, IServiceRegistryRepository>, IServiceRegistryService
    {
        public ServiceRegistryService(IServiceRegistryRepository repo, IMapper mapper) 
            : base(repo, mapper)
        {
        }
    }
}
