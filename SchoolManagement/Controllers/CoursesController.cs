using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService) 
        {
            _courseService = courseService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAvailableCourse()
        {
            return Ok(await _courseService.GetCourses());
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetbyId(int id)
        {
            return Ok(await _courseService.GetCourseById(id));
        }
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateNewCourse(CourseDto course)
        {
            return Ok(await _courseService.AddCourse(course));
        }
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdateCourse(int id, CourseDto course)
        {
            return Ok(await _courseService.UpdateCourse(id,course));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> RemoveCourse(int id)
        {
            return Ok(await _courseService.DeleteCourse(id));
        }
    }
}
