using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories
{
    public interface ITokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByToken(string token);
    }
}
