using FluentValidation;
using DesafioBackEnd.Domain.Extensions;
using DesafioBackEnd.Application.Common;

namespace DesafioBackEnd.Application.Deliveryman.Rent;

public class RentCommandValidator: AbstractValidator<RentCommand>
{
    public RentCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty();

        RuleFor(c => c.StartDay)
            .NotEmpty()
            .Must((model, field) => field < model.EndDay)
            .WithMessage(ValidationMessages.InvalidDate());

        RuleFor(c => c.EndDay)
            .NotEmpty()
            .Must((model, field) => field > model.StartDay && field < model.StartDay.ResetTimeToEndOfDay().AddDays(30))
            .WithMessage(ValidationMessages.InvalidDate());
    }
}
