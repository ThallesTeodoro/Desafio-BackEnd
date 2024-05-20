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

        decimal total;

        if (returnRentDay == rent.EndDay)
        {
            total = rent.Plan.Days * rent.Plan.Value;
        }
        else if (returnRentDay > rent.EndDay)
        {
            var diffInDays = (decimal)((returnRentDay.ConvertToDateTime() - rent.EndDay.ConvertToDateTime()).TotalDays * 50);
            total = (rent.Plan.Days * rent.Plan.Value) + diffInDays;
        }
        else
        {
            var days = (rent.EndDay.ConvertToDateTime() - returnRentDay.ConvertToDateTime()).TotalDays;
            decimal unpaidDailyValue = (decimal)days * rent.Plan.Value;

            var dailyDays = (returnRentDay.ConvertToDateTime() - rent.StartDay.ConvertToDateTime()).TotalDays;
            var dailyValue = (decimal)dailyDays * rent.Plan.Value;

            decimal finePercent = rent.Plan.FinePercent;
            var percent = finePercent / 100 * unpaidDailyValue;

            total = dailyValue + percent;
        }

        return new ReturnRentResponse()
        {
            TotalRentValue = total,
        };
    }
}
