using Bogus.DataSets;
using DesafioBackEnd.Application.Deliveryman.ReturnRent;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using DesafioBackEnd.Domain.Extensions;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Deliveryman;

public class ReturnRentCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;

    public ReturnRentCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _rentRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnCorrectTotalValue_WhenCallHandleForPlanOf7Days()
    {
        var userId = Guid.NewGuid();
        var planId = Guid.NewGuid();
        _userRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.User()
            {
                Id = userId,
                Email = "email@email.com",
                Name = "User Name",
            });

        // Plan - 7 days
        var plan7Days = new Plan()
        {
            Id = Guid.NewGuid(),
            Days = 7,
            Value = 30,
            FinePercent = 20,
        };

        // when return day is equal to end day on 7 days plan should return 210
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            DateTime.Now.AddDays(4),
            210
        );

        // when return day is greater than end day on 7 days plan
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            DateTime.Now.AddDays(6),
            310
        );

        // when return day is less than end day on 7 days plan
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(4)),
            DateTime.Now.AddDays(2),
            132
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnCorrectTotalValue_WhenCallHandleForPlanOf15Days()
    {
        var userId = Guid.NewGuid();
        var planId = Guid.NewGuid();
        _userRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.User()
            {
                Id = userId,
                Email = "email@email.com",
                Name = "User Name",
            });

        // Plan - 15 days
        var plan7Days = new Plan()
        {
            Id = Guid.NewGuid(),
            Days = 15,
            Value = 28,
            FinePercent = 40,
        };

        // when return day is equal to end day on 15 days plan should return 420
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(13)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(13)),
            DateTime.Now.AddDays(13),
            420
        );

        // when return day is greater than end day on 15 days plan
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(13)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(13)),
            DateTime.Now.AddDays(15),
            520
        );

        // when return day is less than end day on 15 days plan
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(13)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(13)),
            DateTime.Now.AddDays(11),
            (decimal)386.4
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnCorrectTotalValue_WhenCallHandleForPlanOf30Days()
    {
        var userId = Guid.NewGuid();
        var planId = Guid.NewGuid();
        _userRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.User()
            {
                Id = userId,
                Email = "email@email.com",
                Name = "User Name",
            });

        // Plan - 30 days
        var plan7Days = new Plan()
        {
            Id = Guid.NewGuid(),
            Days = 30,
            Value = 22,
            FinePercent = 60,
        };

        // when return day is equal to end day on 30 days plan should return 660
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(28)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(28)),
            DateTime.Now.AddDays(28),
            660
        );

        // when return day is greater than end day on 30 days plan
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(28)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(28)),
            DateTime.Now.AddDays(30),
            760
        );

        // when return day is less than end day on 30 days plan
        await RunTestToCase(
            userId,
            planId,
            plan7Days,
            DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(28)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(28)),
            DateTime.Now.AddDays(26),
            (decimal)642.4
        );
    }

    private async Task RunTestToCase(
        Guid userId,
        Guid planId,
        Plan plan,
        DateOnly startDay,
        DateOnly endDay,
        DateOnly prevDay,
        DateTime rentPrevDay,
        decimal checkValue)
    {
        ReturnRentCommandHandler handler = SetupRentMock(userId, planId, plan, startDay, endDay, prevDay);

        var response = await handler.Handle(
            new ReturnRentCommand(
                userId,
                rentPrevDay),
            default);

        response.TotalRentValue.Should().Be(checkValue);
    }

    private ReturnRentCommandHandler SetupRentMock(
        Guid userId,
        Guid planId,
        Plan plan,
        DateOnly startDay,
        DateOnly endDay,
        DateOnly prevDay)
    {
        _rentRepositoryMock
            .Setup(x => x.FindCurrentUserRentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Rent()
            {
                BikeId = Guid.NewGuid(),
                PlanId = planId,
                UserId = userId,
                StartDay = startDay,
                EndDay = endDay,
                PrevDay = prevDay,
                Status = RentStatusEnum.Leased,
                Plan = plan,
            });

        var handler = new ReturnRentCommandHandler(_userRepositoryMock.Object, _rentRepositoryMock.Object);
        return handler;
    }
}
