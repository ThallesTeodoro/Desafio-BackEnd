using DesafioBackEnd.Application.Common;
using DesafioBackEnd.Domain.Contracts.Persistence;
using FluentValidation;

namespace DesafioBackEnd.Application.Bikes.Create;

public class CreateBikeCommandValidator : AbstractValidator<CreateBikeCommand>
{
    public CreateBikeCommandValidator(IBikeRepository bikeRepository)
    {
        RuleFor(c => c.Year)
            .NotEmpty();

        RuleFor(c => c.Type)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.Plate)
            .NotEmpty()
            .MaximumLength(7)
            .MustAsync(async (plate, _) => await bikeRepository.BikePlateIsUniqueAsync(plate))
            .WithMessage(ValidationMessages.ExistenteBikePlate());
    }
}
