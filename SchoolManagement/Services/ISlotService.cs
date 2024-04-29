using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Services
{
    public interface ISlotService
    {
        Task<Slot> AddSlot(SlotDto request);
        Task<Slot> UpdateSlot(int id, SlotDto request);
        Task<IEnumerable<Slot>> ViewAllSlots();
        Task<Slot> ViewSlot(int id);
        Task<bool> DeleteSlot(int id);
    }
}
