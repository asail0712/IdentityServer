using AutoMapper;

namespace Common.DTO.Login
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginRequest, LoginEntity>();
            CreateMap<LoginEntity, LoginResponse>();
            CreateMap<LoginEntity, LoginDocument>();
            CreateMap<LoginDocument, LoginEntity>();
        }
    }

}
