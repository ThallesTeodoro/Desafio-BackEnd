using DesafioBackEnd.Application.Deliveryman.ReturnRent;
using MediatR;

namespace DesafioBackEnd.Application.Deliveryman.ReturnBike;

public record ReturnBikeCommand(Guid UserId) : IRequest<ReturnRentResponse>;
