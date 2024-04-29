using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        Task<List<WeeklyScheduleDto>> GetScheduleByUserIdAndWeek(int userId, int weekOffset);
        Task<List<WeeklyScheduleDto>> GetSchedulesByUserId(int userId);
        Task<List<Schedule>> GetSchedulesByClassId(int classId);
        Task<List<Schedule>> GetSchedulesOfUserFromStartDateToEndDate(int userId, DateTime startDate, DateTime endDate);
        Task<bool> CheckExistSlotInScheduleDate(int scheduleId,int slotId, DateTime scheduleDate);
    }
}
