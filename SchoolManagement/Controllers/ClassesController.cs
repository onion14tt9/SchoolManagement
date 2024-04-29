using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _classService.GetClasses());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            return Ok(await _classService.GetClassById(id));
        }
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetbyCourse(int courseId)
        {
            return Ok(await _classService.GetClassesByCourse(courseId));
        }
        [HttpPost("{courseId}")]
        public async Task<IActionResult> Create(int courseId, ClassDto clazz)
        {
            return Ok(await _classService.AddClass(courseId, clazz));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClassDto clazz)
        {
            return Ok(await _classService.UpdateClass(id, clazz));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            return Ok(await _classService.DeleteClass(id));
        }
    }
}
