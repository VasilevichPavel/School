using AutoMapper;
using School.Entity.Models;
using School.Web.Dto;

namespace School.Web.Profiles
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, GetClassDto>();
            CreateMap<CreateClassDto, Class>();
        }
    }
}
