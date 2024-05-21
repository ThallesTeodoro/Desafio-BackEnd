using DesafioBackEnd.Application.Bikes.List;
using MediatR;

namespace DesafioBackEnd.Application.Bikes.Update;

public record UpdateBikeCommand(Guid Id, string Plate) : IRequest<BikeResponse>;
