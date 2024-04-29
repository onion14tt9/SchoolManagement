using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Services
{
    public interface IUserClassService
    {
        Task<List<UserClass>> AssignUsersToClass(List<int> userIds, int classId);
        Task<List<ClassDto>> ViewAllAssignedClasses();
        Task<UserClass> AssignClassToUser(int userId, int classId);
        Task<List<User>> ViewStudentInsideSameAssignedClass(int classId);
        Task<StudentGradeDto> AddGradeForStudentByClass(AddStudentGradeDto request);
        Task<List<StudentGradeDto>> DisplayStudentGradeInsideClass(int classId);
        Task<UserClass> RegisterClassForStudent(int classId);
        Task<UserClass> AcceptRegistrationOfStudent(int userId, int classId);
    }
}
