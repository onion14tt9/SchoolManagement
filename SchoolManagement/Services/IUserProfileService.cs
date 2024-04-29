
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Services
{
    public interface IUserProfileService
    {
        Task<IEnumerable<User>> GetAllTeachers();
        Task<IEnumerable<User>> GetAllStudents();   
        Task<User?> GetUserById(int id);
        Task<List<UserProfileDto>> AddUserProfile(List<UserProfileDto> model);
        Task<UserProfileDto> UpdateUserProfile(int id, UserProfileDto model);
        Task<bool> DeleteUserProfile(int id);
    }
}
