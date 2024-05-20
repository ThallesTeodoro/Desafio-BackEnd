using DesafioBackEnd.Application.Common;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Extensions;
using FluentValidation.Results;
using MediatR;

namespace DesafioBackEnd.Application.Deliveryman.Rent;

public class RentCommandHandler : IRequestHandler<RentCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPlanRepository _planRepository;
    private readonly IBikeRepository _bikeRepository;
    private readonly IRentRepository _rentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RentCommandHandler(
        IUnitOfWork unitOfWork,
        IBikeRepository bikeRepository,
        IPlanRepository planRepository,
        IUserRepository userRepository,
        IRentRepository rentRepository)
    {
        _unitOfWork = unitOfWork;
        _bikeRepository = bikeRepository;
        _planRepository = planRepository;
        _userRepository = userRepository;
        _rentRepository = rentRepository;
    }

    public async Task Handle(RentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);

        // TODO: Validate if user do not have available rent

        if (user is null)
        {
            throw new NotFoundException("User was not found");
        }

        var bike = await _bikeRepository.FindAvailableBikeToRentAsync();

        if (bike is null)
        {
            var failures = new List<ValidationFailure>()
            {
                new ValidationFailure("Bike", ValidationMessages.NoBikeAvailableToRent()),
            };
            throw new ValidationException(failures);
        }

        var endDay = request.EndDay.ResetTimeToEndOfDay();
        var startDay = request.StartDay.ResetTimeToStartOfDay();
        var rentDays = (endDay - startDay).TotalDays;

        var plan = await _planRepository.GetPlanByDaysAsync(Convert.ToInt32(rentDays));

        var rent = new Domain.Entities.Rent()
        {
            Id = Guid.NewGuid(),
            BikeId = bike.Id,
            PlanId = plan.Id,
            UserId = user.Id,
            StartDay = DateOnly.FromDateTime(startDay.AddDays(1)),
            EndDay = DateOnly.FromDateTime(startDay.AddDays(plan.Days + 1)),
            PrevDay = DateOnly.FromDateTime(endDay),
        };

        await _rentRepository.AddAsync(rent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
