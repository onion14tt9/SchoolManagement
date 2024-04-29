using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Services
{
    public interface IUserCourseService
    {
        Task<IEnumerable<UserCourse>> GetAllAssignments();
        Task<UserCourse?> GetAssignmentById(int userId, int courseId);
        Task<UserCourse> AssignCourseToTeacher(UserCourseDto model);
        Task<bool> DeleteAssignment(int userId, int courseId);
        Task<UserCourse> RegisterCourseForStudent(int courseId);
        Task<List<CourseDto>> ViewAllAssignedCourses();
        Task<StudentGradeDto> UpdateGradeForStudentByCourse(UpdateStudentGradeDto request);
        Task<StudentGradeStatusDto> DisplayCourseStatusOfStudent(int studentId, int courseId);
        Task<StudentGradeDto> ViewGradeOfAssignedCourse(int courseId);
        Task<bool> UpdateGradeStatusOfClass(int classId);
    }
}
