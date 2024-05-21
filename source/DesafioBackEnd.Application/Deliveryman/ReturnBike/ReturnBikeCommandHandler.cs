using DesafioBackEnd.Application.Deliveryman.ReturnRent;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Enums;
using DesafioBackEnd.Domain.Extensions;
using MediatR;

namespace DesafioBackEnd.Application.Deliveryman.ReturnBike;

public class ReturnBikeCommandHandler : IRequestHandler<ReturnBikeCommand, ReturnRentResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRentRepository _rentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReturnBikeCommandHandler(
        IUserRepository userRepository,
        IRentRepository rentRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _rentRepository = rentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReturnRentResponse> Handle(ReturnBikeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);

        if (user is null)
        {
            throw new NotFoundException("User was not found");
        }

        var rent = await _rentRepository.FindCurrentUserRentAsync(request.UserId);
        var returnRentDay = DateOnly.FromDateTime(DateTime.UtcNow);

        if (rent is null || returnRentDay.ConvertToDateTime() < rent.StartDay.ConvertToDateTime())
        {
            throw new ForbiddenException("There is no current rent related to this user.");
        }

        decimal total = rent.CalculateTotalToPay(returnRentDay);

        rent.TotalRentValue = total;
        rent.Status = RentStatusEnum.Returned;
        _rentRepository.Update(rent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReturnRentResponse()
        {
            TotalRentValue = total,
        };
    }
}
