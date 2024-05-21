using MediatR;

namespace DesafioBackEnd.Application.Bikes.Create;

public record CreateBikeCommand(short Year, string Type, string Plate) : IRequest;
