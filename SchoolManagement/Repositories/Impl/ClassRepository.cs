using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;

namespace SchoolManagement.Repositories.Impl
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        private readonly SchoolManageDbContext _context;
        public ClassRepository(SchoolManageDbContext dbContext) : base(dbContext)
        {
           _context = dbContext;
        }

        //Implement Generic 
        public override async Task<Class> AddEntity(Class entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                throw new BadRequestException("Cannot add class");
            }
        }

        public override async Task<bool> DeleteEntity(int id)
        {
            var existdata = await DbSet.FirstOrDefaultAsync(item => item.ClassId == id);
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

        public override async Task<List<Class>> GetAllAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<Class> GetAsync(int id)
        {
            return await DbSet.Where(x => !x.IsDeleted).SingleOrDefaultAsync(item => item.ClassId == id);
        }

        public async Task<Class> GetByClassName(string className)
        {
            return await DbSet.Where(x => !x.IsDeleted).SingleOrDefaultAsync(item => item.ClassName == className);
        }

        public async Task<List<Class>> GetByCourse(int courseId)
        {
            return await DbSet.Where(x => !x.IsDeleted && x.CourseId == courseId).ToListAsync();

        }

        public override async Task<Class> UpdateEntity(Class entity)
        {
            try
            {
                DbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            } catch (Exception ex)
            {
                throw new BadRequestException("Cannot update class");
            }
            
        }
    }
}
