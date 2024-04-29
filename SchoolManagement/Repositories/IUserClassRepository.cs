using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories
{
    public interface IUserClassRepository : IGenericRepository<UserClass>
    {
        Task<List<int>> GetClassessByUserId(int userId);
        Task<List<int>> GetStudentsByClassId(int classId);
        Task<UserClass> GetUserClassAsyncById(int userId, int classId);
        Task<StudentGradeDto> GetLatestStudentGrade(int userId, int courseId);
        Task<List<StudentGradeDto>> GetStudentGradesByClassId(int classId);
        Task<UserClass> GetUserClassNotAcceptedAsyncById(int userId, int classId);

    }
}
