using AutoMapper;
using DesafioBackEnd.Application.Bikes.List;
using DesafioBackEnd.Application.Common;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using FluentValidation.Results;
using MediatR;

namespace DesafioBackEnd.Application.Bikes.Update;

public class UpdateBikeCommandHandler : IRequestHandler<UpdateBikeCommand, BikeResponse>
{
    private readonly IBikeRepository _bikeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBikeCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IBikeRepository bikeRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _bikeRepository = bikeRepository;
    }

    public async Task<BikeResponse> Handle(UpdateBikeCommand request, CancellationToken cancellationToken)
    {
        var bike = await _bikeRepository.FindByIdAsync(request.Id);

        if (bike is null)
        {
            throw new NotFoundException("Bike was not found");
        }

        if (bike.Plate.ToUpper() != request.Plate.ToUpper())
        {
            var plateIsUnique = await _bikeRepository.BikePlateIsUniqueAsync(request.Plate, bike.Id);

            if (!plateIsUnique)
            {
                var failures = new List<ValidationFailure>()
                {
                    new ValidationFailure(nameof(request.Plate), ValidationMessages.ExistenteBikePlate()),
                };
                throw new ValidationException(failures);
            }

            bike.Plate = request.Plate.ToUpper();
            bike.UpdatedAt = DateTime.UtcNow;
            _bikeRepository.Update(bike);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return _mapper.Map<BikeResponse>(bike);
    }
}
