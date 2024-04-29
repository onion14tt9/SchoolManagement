using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Services;
using SchoolManagement.Services.Impl;
using System.Security.Claims;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfilesController(IUserProfileService userProfileService) 
        {
            _userProfileService = userProfileService;
        }
        [HttpGet("teachers")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GetAllTeachers()
        {
            return Ok(await _userProfileService.GetAllTeachers());
        }
        [HttpGet("students")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> GetAllStudents()
        {
            return Ok(await _userProfileService.GetAllStudents());
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userProfileService.GetUserById(id));
        }
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateUserList(List<UserProfileDto> profiles)
        {
            return Ok(await _userProfileService.AddUserProfile(profiles));
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, UserProfileDto profile)
        {
            return Ok(await _userProfileService.UpdateUserProfile(id, profile));
        }
        [HttpDelete]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> RemoveUser(int id)
        {
            return Ok(await _userProfileService.DeleteUserProfile(id));
        }
    }
}
