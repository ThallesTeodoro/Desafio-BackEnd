using FluentValidation;

namespace DesafioBackEnd.Application.Bikes.Update;

public class UpdateBikeCommandValidator : AbstractValidator<UpdateBikeCommand>
{
    public UpdateBikeCommandValidator()
    {
        RuleFor(c => c.Plate)
            .NotEmpty()
            .MaximumLength(7);
    }
}
