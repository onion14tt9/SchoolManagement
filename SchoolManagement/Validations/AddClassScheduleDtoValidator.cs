using FluentValidation;
using SchoolManagement.Domain.Dtos;

namespace SchoolManagement.Validations
{
    public class AddClassScheduleDtoValidator : AbstractValidator<AddClassScheduleDto>
    {
        public AddClassScheduleDtoValidator() 
        {
            RuleFor(schedule => schedule.ClassScheduleDtos)
                .NotEmpty().WithMessage("Class Schedule must not be null")
                .Must(list => list.Count == 3).WithMessage("ClassScheduleDtos must have a size of 3");
        }
    }
}
