using AutoMapper;
using DesafioBackEnd.Application.Common;
using DesafioBackEnd.Domain.Contracts.Persistence;
using MediatR;

namespace DesafioBackEnd.Application.Bikes.List;

public class ListBikeQueryHandler : IRequestHandler<ListBikeQuery, PaginationResponse<BikeResponse>>
{
    private readonly IBikeRepository _bikeRepository;
    private readonly IMapper _mapper;

    public ListBikeQueryHandler(IBikeRepository bikeRepository, IMapper mapper)
    {
        _bikeRepository = bikeRepository;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<BikeResponse>> Handle(ListBikeQuery request, CancellationToken cancellationToken)
    {
        var pagination = await _bikeRepository.ListPaginatedAsync(request.Page, request.PageSize, request.Plate);

        return _mapper.Map<PaginationResponse<BikeResponse>>(pagination);
    }
}
