using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories
{
    public interface IUserCourseRepository : IGenericRepository<UserCourse>
    {
        Task<UserCourse> GetAssignmentAsync(int userId, int courseId);
        Task<List<int>> GetCoursesByUserId(int userId);
    }
}
