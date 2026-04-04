using AutoMapper;

namespace Common.DTO.UserProfileService
{
    public class UserProfileServiceProfile : Profile
    {
        public UserProfileServiceProfile()
        {
            CreateMap<UserProfileServiceRequest, UserProfileServiceEntity>();
            CreateMap<UserProfileServiceEntity, UserProfileServiceResponse>();
            CreateMap<UserProfileServiceEntity, UserProfileServiceDocument>();
            CreateMap<UserProfileServiceDocument, UserProfileServiceEntity>();
        }
    }

}
