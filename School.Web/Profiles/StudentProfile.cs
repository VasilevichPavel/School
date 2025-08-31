using AutoMapper;
using School.Entity.Models.People;
using School.Web.Dto;

namespace School.Web.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>();

            CreateMap<Student, GetStudentDto>()
                .ForMember(dest => dest.ClassIds, opt => opt.MapFrom(src => src.Classes.Select(c => c.Id)));
        }
    }
}
