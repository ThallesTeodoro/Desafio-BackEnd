using DesafioBackEnd.Application.Common;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Enums;
using FluentValidation;

namespace DesafioBackEnd.Application.Deliveryman.Register;

public class RegisterDeliverymanCommandValidator : AbstractValidator<RegisterDeliverymanCommand>
{
    public RegisterDeliverymanCommandValidator(IDeliverymanRepository deliverymanRepository, IUserRepository userRepository)
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(c => c.Email)
            .NotEmpty()
            .MaximumLength(255)
            .EmailAddress()
            .MustAsync(async (email, _) => {
                var exists = await userRepository.FindByEmailAsync(email);
                return exists is null;
            })
            .WithMessage(ValidationMessages.ExistenteUserEmail());

        RuleFor(c => c.Cnpj)
            .NotEmpty()
            .MaximumLength(18)
            .MustAsync(async (cnpj, _) => await deliverymanRepository.CnpjIsUniqueAsync(cnpj))
            .WithMessage(ValidationMessages.ExistenteDeliverymanCnpj());

        RuleFor(c => c.Birthdate)
            .NotNull();

        RuleFor(c => c.Cnh)
            .NotEmpty()
            .MaximumLength(9)
            .MustAsync(async (Cnh, _) => await deliverymanRepository.CnhIsUniqueAsync(Cnh))
            .WithMessage(ValidationMessages.ExistenteDeliverymanCnh());

        RuleFor(c => c.CnhType)
            .NotEmpty()
            .MaximumLength(9)
            .Must(cnhTYpe => {
                var validTypes = new List<string>()
                {
                    CnhTypeEnum.GetCnhTypeText(CnhTypeEnum.A),
                    CnhTypeEnum.GetCnhTypeText(CnhTypeEnum.B),
                    CnhTypeEnum.GetCnhTypeText(CnhTypeEnum.AB),
                };

                return validTypes.Contains(cnhTYpe);
            })
            .WithMessage(ValidationMessages.InvalidCnhType());

        RuleFor(c => c.CnhImage)
            .NotNull();
    }
}
