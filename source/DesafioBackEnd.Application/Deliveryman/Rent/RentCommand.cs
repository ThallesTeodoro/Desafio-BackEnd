using MediatR;

namespace DesafioBackEnd.Application.Deliveryman.Rent;

public record RentCommand(Guid UserId, DateTime StartDay, DateTime EndDay) : IRequest;
