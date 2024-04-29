using FluentValidation;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Repositories;

namespace SchoolManagement.Validations
{
    public class SignupDtoValidator : AbstractValidator<SignUpDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SignupDtoValidator(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters")
            .Must(BeUniqueUsername).WithMessage("Username is already exist");
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress()
                .Must(BeUniqueEmail).WithMessage("Email is already exist");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")
                .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one digit.");
        }


        private bool BeUniqueUsername(string username)
        {
            return Task.Run(async () => await BeUniqueUsernameAsync(username)).Result;
        }

        private bool BeUniqueEmail(string email)
        {
            return Task.Run(async () => await BeUniqueEmailAsync(email)).Result;
        }
        private async Task<bool> BeUniqueUsernameAsync(string username)
        {
            var existingUser = await _unitOfWork.UserRepository.GetByUsername(username);
            return existingUser == null;
        }

        private async Task<bool> BeUniqueEmailAsync(string email)
        {
            var existingUser = await _unitOfWork.UserRepository.GetByEmail(email);
            return existingUser == null;
        }
    }
}
