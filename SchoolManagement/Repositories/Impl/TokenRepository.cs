using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories.Impl
{
    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
    {
        private readonly SchoolManageDbContext _context;

        public TokenRepository(SchoolManageDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        //Implement Generic 
        public override async Task<RefreshToken> AddEntity(RefreshToken entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override async Task<bool> DeleteEntity(int id)
        {
            var existdata = await DbSet.FirstOrDefaultAsync(item => item.UserId == id);
            if (existdata != null)
            {
                DbSet.Remove(existdata);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override async Task<List<RefreshToken>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

       


        public override async Task<RefreshToken> UpdateEntity(RefreshToken entity)
        {
            var result = await base.UpdateEntity(entity);
            await _context.SaveChangesAsync();
            return result;
        }
        public async Task<RefreshToken> GetByToken(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}
