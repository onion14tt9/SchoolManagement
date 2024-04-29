using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Services;
using System.Runtime.CompilerServices;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCoursesController : ControllerBase
    {
        private readonly IUserCourseService _userCourseService;

        public UserCoursesController(IUserCourseService userCourseService)
        {
            _userCourseService = userCourseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssignment()
        {
            return Ok(await _userCourseService.GetAllAssignments());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentById(int userId, int courseId)
        {
            return Ok(await _userCourseService.GetAssignmentById(userId, courseId));
        }
        [HttpGet("view-assigned")]
        [Authorize(Roles = nameof(UserRole.Student) + "," + nameof(UserRole.Teacher))]
        public async Task<IActionResult> GetOwnAssignedCourses()
        {
            return Ok(await _userCourseService.ViewAllAssignedCourses());
        }
        [HttpGet("view-status/{studentId}/{courseId}")]
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
        public async Task<IActionResult> DisplayCourseStatusOfStudent(int studentId, int courseId)
        {
            return Ok(await _userCourseService.DisplayCourseStatusOfStudent(studentId, courseId));
        }

        [HttpGet("view-own-grade")]
        [Authorize(Roles = nameof(UserRole.Student))]
        public async Task<IActionResult> ViewGradeOfAssignedCourse(int courseId)
        {
            return Ok(await _userCourseService.ViewGradeOfAssignedCourse(courseId));
        }

        [HttpPost("assign-course")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> AssignCourseToTeacher(UserCourseDto userCourse)
        {
            return Ok(await _userCourseService.AssignCourseToTeacher(userCourse));
        }

        [HttpPost("register-course")]
        [Authorize(Roles = nameof(UserRole.Student))]
        public async Task<IActionResult> RegisterCourseForStudent(int courseId)
        {
            return Ok(await _userCourseService.RegisterCourseForStudent(courseId));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAssignment(int userId, int courseId)
        {
            return Ok(await _userCourseService.DeleteAssignment(userId, courseId));
        }

        [HttpPut("update-grade")]
        [Authorize(Roles = nameof(UserRole.Teacher))]
        public async Task<IActionResult> UpdateGradeForStudentByCourse(UpdateStudentGradeDto request)
        {
            return Ok(await _userCourseService.UpdateGradeForStudentByCourse(request));
        }
        [HttpPut("update-class-status/{classId}")]
        public async Task<IActionResult> UpdateGradeStatusOfClass(int classId)
        {
            return Ok(await _userCourseService.UpdateGradeStatusOfClass(classId));
        }

    }
}
