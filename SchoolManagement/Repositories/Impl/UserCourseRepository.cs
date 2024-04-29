using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories.Impl
{
    public class UserCourseRepository : GenericRepository<UserCourse>, IUserCourseRepository
    {
        private readonly SchoolManageDbContext _context;

        public UserCourseRepository(SchoolManageDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        //Implement Generic 
        public override async Task<UserCourse> AddEntity(UserCourse entity)
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

        public async Task<bool> DeleteEntity(int userId, int courseId)
        {
            var existdata = await DbSet.FirstOrDefaultAsync(item => item.CourseId == courseId && item.UserId == userId);
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

        public override async Task<List<UserCourse>> GetAllAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<UserCourse> GetAssignmentAsync(int userId, int courseId)
        {
            return await DbSet.Where(x => !x.IsDeleted).SingleOrDefaultAsync(item => item.CourseId == courseId && item.UserId == userId);
        }

        public async Task<List<int>> GetCoursesByUserId(int userId)
        {
            return await DbSet
            .Where(uc => uc.UserId == userId && !uc.IsDeleted)
            .Select(uc => uc.CourseId)
            .ToListAsync();
        }

        public override async Task<UserCourse> UpdateEntity(UserCourse entity)
        {
            DbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
