using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<Course> GetByCourseName(string courseName);
    }
}
