using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using MediatR;

namespace DesafioBackEnd.Application.Bikes.Remove;

public class RemoveBikeCommandHandler : IRequestHandler<RemoveBikeCommand>
{
    private readonly IBikeRepository _bikeRepository;
    private readonly IRentRepository _rentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBikeCommandHandler(IUnitOfWork unitOfWork, IBikeRepository bikeRepository, IRentRepository rentRepository)
    {
        _unitOfWork = unitOfWork;
        _bikeRepository = bikeRepository;
        _rentRepository = rentRepository;
    }

    public async Task Handle(RemoveBikeCommand request, CancellationToken cancellationToken)
    {
        var bike = await _bikeRepository.FindByIdAsync(request.bikeId);

        if (bike is null)
        {
            throw new NotFoundException("Bike was not found");
        }

        var thereIsRelatedRent = await _rentRepository.CheckBikeRentAsync(request.bikeId);

        if (thereIsRelatedRent)
        {
            throw new ForbiddenException("Forbidden to remove the bike because there are Rents related to it.");
        }

        _bikeRepository.Remove(bike);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
