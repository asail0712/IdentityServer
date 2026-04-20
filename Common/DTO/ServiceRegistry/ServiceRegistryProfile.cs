using AutoMapper;

namespace Common.DTO.ServiceRegistry
{
    public class ServiceRegistryProfile : Profile
    {
        public ServiceRegistryProfile()
        {
            CreateMap<ServiceRegistryRequest, ServiceRegistryEntity>();
            CreateMap<ServiceRegistryEntity, ServiceRegistryResponse>();
            CreateMap<ServiceRegistryEntity, ServiceRegistryDocument>();
            CreateMap<ServiceRegistryDocument, ServiceRegistryEntity>();
        }
    }

}
