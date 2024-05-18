using DesafioBackEnd.Application.Common;
using FluentValidation;

namespace DesafioBackEnd.Application.User.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
