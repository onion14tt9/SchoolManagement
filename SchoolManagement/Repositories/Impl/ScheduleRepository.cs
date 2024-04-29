using LinqToDB.SqlQuery;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;

namespace SchoolManagement.Repositories.Impl
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        private readonly SchoolManageDbContext _context;

        public ScheduleRepository(SchoolManageDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override async Task<Schedule> AddEntity(Schedule entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                throw new BadRequestException("Cannot add schedule");
            }
        }

        public async Task<bool> CheckExistSlotInScheduleDate(int scheduleId, int slotId, DateTime scheduleDate)
        {
            return await DbSet
           .Join(_context.ClassScheduleSlots,
           s => s.ScheduleId,
           csl => csl.ScheduleId,
           (s, csl) => new { Schedule = s, ClassScheduleSlot = csl })
           .Join(_context.Slots,
           joined => joined.ClassScheduleSlot.SlotId,
           sl => sl.SlotId,
           (joined, sl) => new { joined.Schedule, joined.ClassScheduleSlot, Slot = sl })
           .Where(joined => !joined.Schedule.IsDeleted && joined.Schedule.ScheduleId == scheduleId && joined.Schedule.ScheduleDate == scheduleDate && joined.Slot.SlotId == slotId)
           .AnyAsync();
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

        public override async Task<List<Schedule>> GetAllAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<Schedule> GetAsync(int id)
        {
            return await DbSet.Where(x => !x.IsDeleted).SingleOrDefaultAsync(item => item.ScheduleId == id);
        }

        public async Task<List<WeeklyScheduleDto>> GetScheduleByUserIdAndWeek(int userId, int weekOffset)
        {
            DateTime currentDate = DateTime.Now;
            DateTime targetWeekStart = currentDate.AddDays(7 * weekOffset).Date;

            return await DbSet
            .Join(_context.ClassScheduleSlots,
            s => s.ScheduleId,
            csl => csl.ScheduleId,
            (s, csl) => new { Schedule = s, ClassScheduleSlot = csl })
            .Join(_context.Slots,
            joined => joined.ClassScheduleSlot.SlotId,
            sl => sl.SlotId,
            (joined, sl) => new { joined.Schedule, joined.ClassScheduleSlot, Slot = sl })
            .Join(_context.Classes,
            joined => joined.Schedule.ClassId,
            c => c.ClassId,
            (joined, c) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, Class = c })
            .Join(_context.UserClasses,
            joined => joined.Class.ClassId,
            uc => uc.ClassId,
            (joined, uc) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, joined.Class, UserClass = uc })
            .Join(_context.Users,
            joined => joined.UserClass.UserId,
            u => u.UserId,
            (joined, u) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, joined.Class, joined.UserClass, User = u })
            .Where(joined => joined.User.UserId == userId
            && EF.Functions.DateDiffWeek(joined.Schedule.ScheduleDate, targetWeekStart) == 0
            )
            .OrderBy(joined => joined.Schedule.ScheduleDate)         
            .ThenBy(joined => joined.Slot.StartDate)
            .Select(joined => new WeeklyScheduleDto
            {
                WeekDay = (int)joined.Schedule.ScheduleDate.DayOfWeek,
                ScheduleDate = joined.Schedule.ScheduleDate,
                ClassId = joined.Class.ClassId,
                ClassName = joined.Class.ClassName,
                SlotId = joined.Slot.SlotId
            })
            .ToListAsync();
        }

        public async Task<List<Schedule>> GetSchedulesByClassId(int classId)
        {
            return await DbSet.Where(x => !x.IsDeleted && x.ClassId == classId).ToListAsync();
        }

        public async Task<List<WeeklyScheduleDto>> GetSchedulesByUserId(int userId)
        {
            return await DbSet
            .Join(_context.ClassScheduleSlots,
            s => s.ScheduleId,
            csl => csl.ScheduleId,
            (s, csl) => new { Schedule = s, ClassScheduleSlot = csl })
            .Join(_context.Slots,
            joined => joined.ClassScheduleSlot.SlotId,
            sl => sl.SlotId,
            (joined, sl) => new { joined.Schedule, joined.ClassScheduleSlot, Slot = sl })
            .Join(_context.Classes,
            joined => joined.Schedule.ClassId,
            c => c.ClassId,
            (joined, c) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, Class = c })
            .Join(_context.UserClasses,
            joined => joined.Class.ClassId,
            uc => uc.ClassId,
            (joined, uc) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, joined.Class, UserClass = uc })
            .Join(_context.Users,
            joined => joined.UserClass.UserId,
            u => u.UserId,
            (joined, u) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, joined.Class, joined.UserClass, User = u })
            .Where(joined => joined.User.UserId == userId
              && joined.Schedule.IsDeleted == false
            )
            .OrderBy(joined => joined.Schedule.ScheduleDate)
            .ThenBy(joined => joined.Slot.StartDate)
            .Select(joined => new WeeklyScheduleDto
            {
                WeekDay = (int)joined.Schedule.ScheduleDate.DayOfWeek,
                ScheduleDate = joined.Schedule.ScheduleDate,
                ClassId = joined.Class.ClassId,
                ClassName = joined.Class.ClassName,
                SlotId = joined.Slot.SlotId
            })
            .ToListAsync();
        }

        public async Task<List<Schedule>> GetSchedulesOfUserFromStartDateToEndDate(int userId, DateTime startDate, DateTime endDate)
        {
            return await DbSet
            .Join(_context.ClassScheduleSlots,
            s => s.ScheduleId,
            csl => csl.ScheduleId,
            (s, csl) => new { Schedule = s, ClassScheduleSlot = csl })
            .Join(_context.Slots,
            joined => joined.ClassScheduleSlot.SlotId,
            sl => sl.SlotId,
            (joined, sl) => new { joined.Schedule, joined.ClassScheduleSlot, Slot = sl })
            .Join(_context.Classes,
            joined => joined.Schedule.ClassId,
            c => c.ClassId,
            (joined, c) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, Class = c })
            .Join(_context.UserClasses,
            joined => joined.Class.ClassId,
            uc => uc.ClassId,
            (joined, uc) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, joined.Class, UserClass = uc })
            .Join(_context.Users,
            joined => joined.UserClass.UserId,
            u => u.UserId,
            (joined, u) => new { joined.Schedule, joined.ClassScheduleSlot, joined.Slot, joined.Class, joined.UserClass, User = u })
            .Where(joined => joined.User.UserId == userId 
            && joined.Schedule.ScheduleDate >= startDate && joined.Schedule.ScheduleDate <= endDate)
            .Select(joined => joined.Schedule)
            .ToListAsync();
        }

        public override async Task<Schedule> UpdateEntity(Schedule entity)
        {
            try
            {
                DbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            } catch (Exception ex)
            {
                throw new BadRequestException("Cannot update schedule");
            }
           
        }
    }
}
