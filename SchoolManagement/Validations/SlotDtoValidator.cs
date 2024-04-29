using FluentValidation;
using SchoolManagement.Domain.Dtos;

namespace SchoolManagement.Validations
{
    public class SlotDtoValidator : AbstractValidator<SlotDto>
    {
        public SlotDtoValidator() 
        {
            RuleFor(slotDto => slotDto.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .Must((slotDto, startDate) => startDate < slotDto.DueDate)
                .WithMessage("Start date must be earlier than due date")
                .Must((slotDto, startDate) => IsValidTimeDifference(startDate, slotDto.DueDate))
                .WithMessage("The difference between StartDate and DueDate must be 1 hour and 30 minutes");
        }
        private bool IsValidTimeDifference(TimeSpan startDate, TimeSpan dueDate)
        {
            TimeSpan timeDifference = dueDate - startDate;
            TimeSpan expectedDifference = TimeSpan.FromMinutes(90); // 1 hour and 30 minutes

            return timeDifference == expectedDifference;
        }
    }
}
