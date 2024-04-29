using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Services;
using SchoolManagement.Services.Impl;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotsController(ISlotService slotService) 
        {
            _slotService = slotService;
        }
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GetAllSlots()
        {
            return Ok(await _slotService.ViewAllSlots());
        }
        [HttpGet("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GetbyId(int id)
        {
            return Ok(await _slotService.ViewSlot(id));
        }
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateNewSLot(SlotDto slot)
        {
            return Ok(await _slotService.AddSlot(slot));
        }
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateSlot(int id, SlotDto slot)
        {
            return Ok(await _slotService.UpdateSlot(id, slot));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> RemoveSlot(int id)
        {
            return Ok(await _slotService.DeleteSlot(id));
        }
    }
}
