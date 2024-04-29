using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories
{
    public interface IClassScheduleSlotRepository : IGenericRepository<ClassScheduleSlot>
    {
        Task<ClassScheduleSlot> GetClassScheduleSlotAsync(int classScheduleId, int slotId);
        Task<ClassScheduleSlot> GetClassScheduleSlotByScheduleId(int scheduleId);
    }
}
