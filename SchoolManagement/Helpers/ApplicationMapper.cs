using AutoMapper;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace SchoolManagement.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Class, ClassDto>().ReverseMap();
            CreateMap<UserCourse, UserCourseDto>().ReverseMap();
            CreateMap<User, UserProfileDto>().ReverseMap();
            CreateMap<UserClass, UserClassDto>().ReverseMap();
            CreateMap<StudentGradeDto, StudentGradeStatusDto>().ReverseMap();
            CreateMap<SlotDto, Slot>().ReverseMap();
        }
    }
}
