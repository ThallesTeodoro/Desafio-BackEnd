using MediatR;

namespace DesafioBackEnd.Application.User.Login;

public record LoginCommand(string Email) : IRequest<LoginResponse>;
