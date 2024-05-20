using FluentValidation;
using DesafioBackEnd.Domain.Extensions;
using DesafioBackEnd.Application.Common;

namespace DesafioBackEnd.Application.Deliveryman.ReturnRent;

public class ReturnRentCommandValidator : AbstractValidator<ReturnRentCommand>
{
    public ReturnRentCommandValidator()
    {
        RuleFor(c => c.PrevEndDay)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now.ResetTimeToStartOfDay())
            .WithMessage(ValidationMessages.InvalidDate());
    }
}
