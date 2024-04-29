using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;

namespace SchoolManagement.Repositories.Impl
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly SchoolManageDbContext _context;

        public CourseRepository(SchoolManageDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        //Implement Generic 
        public override async Task<Course> AddEntity(Course entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                throw new BadRequestException("Cannot add course");
            }
        }

        public override async Task<bool> DeleteEntity(int id)
        {
            var existdata = await DbSet.FirstOrDefaultAsync(item => item.CourseId == id);
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

        public override async Task<List<Course>> GetAllAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<Course> GetAsync(int id)
        {
            return await DbSet.Where(x => !x.IsDeleted).SingleOrDefaultAsync(item => item.CourseId == id);
        }

        public async Task<Course> GetByCourseName(string courseName)
        {
            return await DbSet.Where(x => !x.IsDeleted).SingleOrDefaultAsync(item => item.CourseName == courseName);
        }

        public override async Task<Course> UpdateEntity(Course entity)
        {
            try
            {
                DbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            } catch (Exception ex)
            {
                throw new BadRequestException("Cannot update course");
            }
            
        }
    }
}
