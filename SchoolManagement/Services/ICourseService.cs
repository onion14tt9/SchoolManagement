using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCourses();
        Task<Course?> GetCourseById(int id);
        Task<Course> AddCourse(CourseDto model);
        Task<Course> UpdateCourse(int id, CourseDto model);
        Task<bool> DeleteCourse(int id);
    }
}
