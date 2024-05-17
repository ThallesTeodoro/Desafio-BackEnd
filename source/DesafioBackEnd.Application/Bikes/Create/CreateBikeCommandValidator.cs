using DesafioBackEnd.Application.Common;
using DesafioBackEnd.Domain.Contracts.Persistence;
using FluentValidation;

namespace DesafioBackEnd.Application.Bikes.Create;

public class CreateBikeCommandValidator : AbstractValidator<CreateBikeCommand>
{
    public CreateBikeCommandValidator(IBikeRepository bikeRepository)
    {
        RuleFor(c => c.Year)
            .NotEmpty()
            .WithMessage(ValidationMessages.RequiredField(nameof(CreateBikeCommand.Year)));

        RuleFor(c => c.Type)
            .NotEmpty()
            .WithMessage(ValidationMessages.RequiredField(nameof(CreateBikeCommand.Type)));

        RuleFor(c => c.Plate)
            .NotEmpty()
            .WithMessage(ValidationMessages.RequiredField(nameof(CreateBikeCommand.Plate)))
            .MustAsync(async (plate, _) => await bikeRepository.BikePlateIsUniqueAsync(plate))
            .WithMessage(ValidationMessages.ExistenteBikePlate());
    }
}
