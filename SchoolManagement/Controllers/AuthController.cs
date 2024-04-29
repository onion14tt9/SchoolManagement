using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(SignUpDto request)
        {
            return Ok(await  _authService.Register(request));
        }
        [HttpPut("verify-confirm")]
        public async Task<IActionResult> VerifyConfirmForRegister(string email)
        {
            return Ok(await _authService.VerifyConfirmForRegiser(email));
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(SignInDto request)
        {
            return Ok(await _authService.Login(request));
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(OtpVerifyDto request)
        {
            return Ok(await _authService.VerifyOtpForResetPassword(request));
        }

        [HttpPut("resend-otp")]
        public async Task<IActionResult> ResendOtp(string email)
        {
            return Ok(await _authService.ResendOtp(email));
        }
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto request)
        {
            return Ok(await _authService.ResetPassword(request));
        }


        [HttpPost("renew-token")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            return Ok(await _authService.RenewToken(model));
        }
    }
}
