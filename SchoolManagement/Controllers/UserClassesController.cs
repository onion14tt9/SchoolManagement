using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserClassesController : ControllerBase
    {
        private readonly IUserClassService _userClassService;

        public UserClassesController(IUserClassService userClassService) 
        {
            _userClassService = userClassService;
        }
        [HttpPost("assign-users")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> AssignUsersToClass(List<int> userIds, int classId)
        {
            return Ok(await _userClassService.AssignUsersToClass(userIds, classId));
        }
        [HttpPost("assign-class")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> AssignClassToUser(int userId, int classId)
        {
            return Ok(await _userClassService.AssignClassToUser(userId, classId));
        }
        [HttpPost("register-class")]
        [Authorize(Roles = nameof(UserRole.Student))]
        public async Task<IActionResult> RegisterClass(int classId)
        {
            return Ok(await _userClassService.RegisterClassForStudent(classId));
        }

        [HttpGet("view-assigned")]
        [Authorize(Roles = nameof(UserRole.Student) + "," + nameof(UserRole.Teacher))]
        public async Task<IActionResult> ViewAllAssignedClasses()
        {
            return Ok(await _userClassService.ViewAllAssignedClasses());
        }

        [HttpGet("view-assigned-students")]
        [Authorize(Roles = nameof(UserRole.Teacher))]
        public async Task<IActionResult> ViewStudentInsideSameAssignedClass(int classId)
        {
            return Ok(await _userClassService.ViewStudentInsideSameAssignedClass(classId));
        }
        [HttpGet("view-class-grades/{classId}")]
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
        public async Task<IActionResult> DisplayStudentGradeInsideClass(int classId)
        {
            return Ok(await _userClassService.DisplayStudentGradeInsideClass(classId));
        }

        [HttpPut("add-grade")]
        [Authorize(Roles = nameof(UserRole.Teacher))]
        public async Task<IActionResult> AddGradeForStudentByClass(AddStudentGradeDto request)
        {
            return Ok(await _userClassService.AddGradeForStudentByClass(request));
        }
        [HttpPut("accept-registration/{userId}/{classId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> AcceptRegistrationOfStudent(int userId, int classId)
        {
            return Ok(await _userClassService.AcceptRegistrationOfStudent(userId, classId));
        }
    }
}
