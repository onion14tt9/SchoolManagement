using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Services
{
    public interface IAuthService
    {
        Task<User> Register(SignUpDto request);
        Task<bool> VerifyConfirmForRegiser(string email);
        Task<TokenModel> Login(SignInDto request);
        Task<TokenModel> RenewToken(TokenModel model);
        Task<bool> ResetPassword(ResetPasswordDto request);
        Task<bool> VerifyOtpForResetPassword(OtpVerifyDto request);
        Task<bool> ResendOtp(string email);
        Task SendChangePasswordRemindingMail();
    }
}
