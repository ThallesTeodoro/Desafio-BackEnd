using MediatR;

namespace DesafioBackEnd.Application.User.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<UserResponse>;
