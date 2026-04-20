using AutoMapper;

namespace Common.DTO.ServiceToken
{
    public class ServiceTokenProfile : Profile
    {
        public ServiceTokenProfile()
        {
            CreateMap<ServiceTokenRequest, ServiceTokenEntity>();
            CreateMap<ServiceTokenEntity, ServiceTokenResponse>();
            CreateMap<ServiceTokenEntity, ServiceTokenDocument>();
            CreateMap<ServiceTokenDocument, ServiceTokenEntity>();
        }
    }

}
