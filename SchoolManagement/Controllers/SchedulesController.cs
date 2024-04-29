using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService) 
        {
            _scheduleService = scheduleService;
        }
        [HttpGet("view-weekly/{weekOffset}")]
        [Authorize(Roles = nameof(UserRole.Student))]
        public async Task<IActionResult> ViewWeeklySchedule(int weekOffset)
        {
            return Ok(await _scheduleService.ViewWeeklySchedule(weekOffset));
        }
        [HttpGet("{scheduleId}")]
        [Authorize(Roles = nameof(UserRole.Student))]
        public async Task<IActionResult> ViewScheduleById(int scheduleId)
        {
            return Ok(await _scheduleService.GetScheduleById(scheduleId));
        }
        [HttpGet("generate-schedule")]
        [Authorize(Roles = nameof(UserRole.Student))]
        public async Task<IActionResult> GenerateWeeklySchedule()
        {
            return Ok(await _scheduleService.GenerateWeeklySchedule());
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> AddScheduleForClass(AddClassScheduleDto request)
        {
            return Ok(await _scheduleService.AddScheduleForClassInSemester(request));
        }
    }
}
