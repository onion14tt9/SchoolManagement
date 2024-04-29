using FluentValidation;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Repositories;

namespace SchoolManagement.Validations
{
    public class CourseDtoValidator : AbstractValidator<CourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(courseDto => courseDto.CourseName)
                .NotEmpty().WithMessage("Course name is required")
                .MaximumLength(50).WithMessage("Course name must not exceed 50 characters")
                .Must(BeUniqueCourseName).WithMessage("Course name already exists");

            RuleFor(courseDto => courseDto.LessonQuantity)
                .GreaterThan(0).WithMessage("Lesson quantity must be greater than 0")
                .LessThan(100).WithMessage("Lesson quantity must be less than 100");
        }

        private bool BeUniqueCourseName(string courseName)
        {
            return Task.Run(async () => await BeUniqueCourseNameAsync(courseName)).Result;
        }
        private async Task<bool> BeUniqueCourseNameAsync(string courseName)
        {
            var existingUser = await _unitOfWork.CourseRepository.GetByCourseName(courseName);
            return existingUser == null;
        }
    }
}