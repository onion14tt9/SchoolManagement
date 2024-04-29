using FluentValidation;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Repositories;

namespace SchoolManagement.Validations
{
    public class ClassDtoValidator :AbstractValidator<ClassDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClassDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(user => user.ClassName)
               .NotEmpty().WithMessage("Class name is required")
               .MaximumLength(50).WithMessage("Class name must not exceed 50 characters")
               .Must(BeUniqueClassName).WithMessage("Class name is already exist");
            RuleFor(classDto => classDto.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .Must((classDto, startDate) => startDate < classDto.DueDate)
                .WithMessage("Start date must be earlier than end date")
                .Must((classDto, startDate) => DueDateWithinThreeMonths(startDate, classDto.DueDate))
                .WithMessage("Due date must be within 3 months from start date"); 
        }

        private bool DueDateWithinThreeMonths(DateTime startDate, DateTime dueDate)
        {
            DateTime maxDueDate = startDate.AddMonths(3);
            return dueDate <= maxDueDate;
        }


        private bool BeUniqueClassName(string className)
        {
            return Task.Run(async () => await BeUniqueClassNameAsync(className)).Result;
        }
        private async Task<bool> BeUniqueClassNameAsync(string className)
        {
            var existingUser = await _unitOfWork.ClassRepository.GetByClassName(className);
            return existingUser == null;
        }
    }
}

