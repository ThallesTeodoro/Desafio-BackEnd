using FluentValidation;

namespace DesafioBackEnd.Application.Order.RegisterOrder;

public class RegisterOrderCommandValidator : AbstractValidator<RegisterOrderCommand>
{
    public RegisterOrderCommandValidator()
    {
        RuleFor(c => c.Value)
            .NotNull();
    }
}
