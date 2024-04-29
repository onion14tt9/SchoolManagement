using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;

namespace SchoolManagement.Repositories.Impl
{
    public class ClassScheduleSlotRepository : GenericRepository<ClassScheduleSlot>, IClassScheduleSlotRepository
    {
        private readonly SchoolManageDbContext _context;

        public ClassScheduleSlotRepository(SchoolManageDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override async Task<ClassScheduleSlot> AddEntity(ClassScheduleSlot entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                throw new BadRequestException("Cannot add class schedule");
            }
        }

        public override async Task<bool> DeleteEntity(int id)
        {
            var existdata = await DbSet.FirstOrDefaultAsync(item => item.ScheduleId == id);
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

        public override async Task<List<ClassScheduleSlot>> GetAllAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<ClassScheduleSlot> GetClassScheduleSlotAsync(int classScheduleId, int slotId)
        {
            return await DbSet.Where(x => !x.IsDeleted).SingleOrDefaultAsync(item => item.ScheduleId == classScheduleId && item.SlotId == slotId);
        }

        public async Task<ClassScheduleSlot> GetClassScheduleSlotByScheduleId(int scheduleId)
        {
            return await DbSet.Where(x => !x.IsDeleted).FirstOrDefaultAsync(item => item.ScheduleId == scheduleId);
        }

        public override async Task<ClassScheduleSlot> UpdateEntity(ClassScheduleSlot entity)
        {
            try
            {
                DbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            } catch (Exception ex)
            {
                throw new BadRequestException("Cannot update class schedule");
            }
            
        }
    }
}
