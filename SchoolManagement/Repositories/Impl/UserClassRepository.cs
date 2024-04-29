using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;

namespace SchoolManagement.Repositories.Impl
{
    public class UserClassRepository : GenericRepository<UserClass>, IUserClassRepository
    {
        private readonly SchoolManageDbContext _context;

        public UserClassRepository(SchoolManageDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        //Implement Generic 
        public override async Task<UserClass> AddEntity(UserClass entity)
        {
            try
            {

                await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                throw new BadRequestException("Cannot add user class");
            }
        }

        public async Task<bool> DeleteEntity(int userId, int classId)
        {
            var existdata = await DbSet.FirstOrDefaultAsync(item => item.ClassId == classId && item.UserId == userId);
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

        public override async Task<List<UserClass>> GetAllAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<List<int>> GetClassessByUserId(int userId)
        {
            return await DbSet
            .Where(uc => uc.UserId == userId && !uc.IsDeleted)
            .Select(uc => uc.ClassId)
            .ToListAsync();
        }

        public async Task<StudentGradeDto> GetLatestStudentGrade(int userId, int courseId)
        {
            return await DbSet
            .Where(uc => uc.UserId == userId && uc.IsDeleted == false)
            .Join(
                dbContext.Classes,
                uc => uc.ClassId,
                cl => cl.ClassId,
                (uc, cl) => new { UserClass = uc, Class = cl }
            )
            .Join(
                dbContext.Courses,
                joined => joined.Class.CourseId,
                co => co.CourseId,
                (joined, co) => new { joined.UserClass, joined.Class, Course = co }
            )
            .Join(
                dbContext.Users,
                joined => joined.UserClass.UserId,
                u => u.UserId,
                (joined, u) => new { joined.UserClass, joined.Class, joined.Course, User = u }
            )
            .Where(joined => joined.User.UserId == userId && joined.Course.CourseId == courseId)
            .OrderByDescending(joined => joined.UserClass.CreatedDate)
            .Select(joined => new StudentGradeDto
            {
                UserId = joined.User.UserId,
                ClassId = joined.UserClass.ClassId,
                Name = joined.User.Name,
                Grade = joined.UserClass.Grade
            })
            .FirstOrDefaultAsync();
        }

        public async Task<List<StudentGradeDto>> GetStudentGradesByClassId(int classId)
        {
            return await DbSet
               .Where(uc => uc.ClassId == classId && uc.IsDeleted == false)
               .Join(
                   dbContext.Users,
                   uc => uc.UserId,
                   u => u.UserId,
                   (uc, u) => new { UserClass = uc, User = u }
               )
               .Select(joined => new StudentGradeDto
               {
                   UserId = joined.User.UserId,
                   ClassId = joined.UserClass.ClassId,
                   Name = joined.User.Name,
                   Grade = joined.UserClass.Grade
               })
               .ToListAsync();
        }

        public async Task<List<int>> GetStudentsByClassId(int classId)
        {
            return await DbSet
            .Join(
            dbContext.Users,
            uc => uc.UserId,
            u => u.UserId,
            (uc, u) => new { UserClass = uc, User = u }
            )
            .Where(joined => joined.UserClass.ClassId == classId && !joined.UserClass.IsDeleted && joined.User.Role == UserRole.Student)
            .Select(joined => joined.UserClass.UserId)
            .ToListAsync();
        }

        public async Task<UserClass> GetUserClassAsyncById(int userId, int classId)
        {
            return await DbSet.SingleOrDefaultAsync(item => item.ClassId == classId && item.UserId == userId && !item.IsDeleted);


        }

        public async Task<UserClass> GetUserClassNotAcceptedAsyncById(int userId, int classId)
        {
            return await DbSet.SingleOrDefaultAsync(item => item.ClassId == classId && item.UserId == userId && item.IsDeleted);
        }

        public override async Task<UserClass> UpdateEntity(UserClass entity)
        {
            try
            {
                DbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Cannnot update UserClass");
            }

        }

    }
}
