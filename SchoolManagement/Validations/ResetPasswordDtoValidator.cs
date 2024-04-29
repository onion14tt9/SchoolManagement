using FluentValidation;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Repositories;

namespace SchoolManagement.Validations
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(resetPasswordDto => resetPasswordDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .Must(UserExists).WithMessage("Email does not exist in the system");

            RuleFor(resetPasswordDto => resetPasswordDto.Password)
                .NotEmpty().WithMessage("Password is required");

            RuleFor(resetPasswordDto => resetPasswordDto.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(resetPasswordDto => resetPasswordDto.Password)
                .WithMessage("Confirm password must match the password");
        }

        private bool UserExists(string email)
        {
            return Task.Run(async () => await UserExistsAsync(email)).Result;
        }
        private async Task<bool> UserExistsAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetByEmail(email) != null;
        }
    }
}
