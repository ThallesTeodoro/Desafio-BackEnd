using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Extensions;
using MediatR;

namespace DesafioBackEnd.Application.Deliveryman.ReturnRent;

public class ReturnRentCommandHandler : IRequestHandler<ReturnRentCommand, ReturnRentResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRentRepository _rentRepository;

    public ReturnRentCommandHandler(
        IUserRepository userRepository,
        IRentRepository rentRepository)
    {
        _userRepository = userRepository;
        _rentRepository = rentRepository;
    }

    public async Task<ReturnRentResponse> Handle(ReturnRentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);

        if (user is null)
        {
            throw new NotFoundException("User was not found");
        }

        var rent = await _rentRepository.FindCurrentUserRentAsync(request.UserId);
        var returnRentDay = DateOnly.FromDateTime(request.PrevEndDay);

        if (rent is null || returnRentDay.ConvertToDateTime() < rent.StartDay.ConvertToDateTime())
        {
            throw new ForbiddenException("There is no current rent related to this user.");
        }

        decimal total = rent.CalculateTotalToPay(returnRentDay);

        return new ReturnRentResponse()
        {
            TotalRentValue = total,
        };
    }
}
