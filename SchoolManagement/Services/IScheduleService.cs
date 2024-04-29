using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> AddScheduleForClassInSemester(AddClassScheduleDto request);
        Task<List<WeeklyScheduleDto>> ViewWeeklySchedule(int weekOffset);
        Task<List<WeeklyScheduleDto>> GenerateWeeklySchedule();
        Task<Schedule> GetScheduleById(int scheduleId);
        Task<bool> RemoveSchedulesFromClass(int classId);
    }
}
