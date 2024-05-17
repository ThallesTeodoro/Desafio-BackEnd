using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using MediatR;

namespace DesafioBackEnd.Application.Bikes.Create;

public class CreateBikeCommandHandler : IRequestHandler<CreateBikeCommand>
{
    private readonly IBikeRepository _bikeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBikeCommandHandler(IBikeRepository bikeRepository, IUnitOfWork unitOfWork)
    {
        _bikeRepository = bikeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateBikeCommand request, CancellationToken cancellationToken)
    {
        await _bikeRepository.AddAsync(new Bike()
        {
            Id = Guid.NewGuid(),
            Plate = request.Plate,
            Type = request.Type,
            Year = request.Year,
            CreatedAt = DateTime.UtcNow,
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
