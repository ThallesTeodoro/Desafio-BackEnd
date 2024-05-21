using DesafioBackEnd.Application.Common;
using MediatR;

namespace DesafioBackEnd.Application.Bikes.List;

public record ListBikeQuery(int Page, int PageSize, string? Plate) : IRequest<PaginationResponse<BikeResponse>>;
