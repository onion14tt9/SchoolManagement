using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;

namespace SchoolManagement.Repositories.Impl
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly SchoolManageDbContext _context;

        public UserRepository(SchoolManageDbContext context) : base(context)
        {
            _context = context;
        }

        //Implement Generic 
        public override async Task<User> AddEntity(User entity)
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
                throw new NotFoundException("User not found");
            }
        }

        public override async Task<List<User>> GetAllAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<User> GetAsync(int id)
        {
            return await DbSet.SingleOrDefaultAsync(item => item.UserId == id && !item.IsDeleted && item.IsVerified);
        }


        public override async Task<User> UpdateEntity(User entity)
        {
            DbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(p => p.Username == username && !p.IsDeleted && p.IsVerified);
        }

        public async Task<List<User>> GetAllUsersByRole(UserRole role)
        {
            return await _context.Users.Where(p => p.Role == role && !p.IsDeleted && p.IsVerified).ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(p => p.Email.Equals(email) && !p.IsDeleted && p.IsVerified);
        }

        public async Task<User> GetByName(string name)
        {
            return await _context.Users.SingleOrDefaultAsync(p => p.Name.Equals(name) && !p.IsDeleted && p.IsVerified);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return !await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetAllNonAdminUsers()
        {
            return await DbSet.Where(x => !x.IsDeleted && x.Role!= UserRole.Admin).ToListAsync();
        }
    }
}
