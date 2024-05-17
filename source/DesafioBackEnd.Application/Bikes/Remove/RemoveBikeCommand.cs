using MediatR;

namespace DesafioBackEnd.Application.Bikes.Remove;

public record RemoveBikeCommand(Guid bikeId) : IRequest;
