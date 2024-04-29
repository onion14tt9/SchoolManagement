using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsername(string username);
        Task<User> GetByEmail(string email);
        Task<User> GetByName(string name);
        Task<List<User>> GetAllUsersByRole(UserRole role);
        Task<bool> IsUsernameUniqueAsync(string username);
        Task<List<User>> GetAllNonAdminUsers();
    }
}
