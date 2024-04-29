using AutoMapper;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;
using SchoolManagement.Repositories;

namespace SchoolManagement.Services.Impl
{
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SlotService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Slot> AddSlot(SlotDto request)
        {
            var slot = _mapper.Map<Slot>(request);
            slot.IsDeleted = false;
            return await _unitOfWork.SlotRepository.AddEntity(slot);
        }

        public async Task<bool> DeleteSlot(int id)
        {
            var data = await _unitOfWork.SlotRepository.GetAsync(id) ?? throw new NotFoundException("Slot not found");
            data.IsDeleted = true;
            await _unitOfWork.SlotRepository.UpdateEntity(data);
            return true;
        }

        public async Task<Slot> UpdateSlot(int id, SlotDto request)
        {
            var data = await _unitOfWork.SlotRepository.GetAsync(id) ?? throw new NotFoundException("Slot not found");
            data.StartDate = request.StartDate;
            data.DueDate = request.DueDate;
            var slot = await _unitOfWork.SlotRepository.UpdateEntity(data);
            await _unitOfWork.CompleteAsync();
            return slot;
        }

        public async Task<IEnumerable<Slot>> ViewAllSlots()
        {
            return await _unitOfWork.SlotRepository.GetAllAsync();
        }

        public async Task<Slot> ViewSlot(int id)
        {
            var slot = await _unitOfWork.SlotRepository.GetAsync(id) ?? throw new NotFoundException("Slot not found");
            return slot;
        }
    }
}
