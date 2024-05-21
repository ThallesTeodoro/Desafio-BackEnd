using MediatR;

namespace DesafioBackEnd.Application.Deliveryman.ReturnRent;

public record ReturnRentCommand(Guid UserId, DateTime PrevEndDay) : IRequest<ReturnRentResponse>;
