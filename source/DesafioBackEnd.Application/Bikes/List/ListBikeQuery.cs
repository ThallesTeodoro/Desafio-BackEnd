using DesafioBackEnd.Application.Common;
using MediatR;

namespace DesafioBackEnd.Application.Bikes.List;

public record ListBikeQuery(int Page, int PageSize) : IRequest<PaginationResponse<BikeResponse>>;
