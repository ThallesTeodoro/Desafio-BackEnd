using DesafioBackEnd.Application.Deliveryman.ReturnBike;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Deliveryman;

public class ReturnBikeCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public ReturnBikeCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _rentRepositoryMock = new();
        _unitOfWorkMock = new();
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
            DateOnly.FromDateTime(DateTime.Now.AddDays(-7)),
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now),
            210
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
            DateOnly.FromDateTime(DateTime.Now.AddDays(-15)),
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now),
            420
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
            DateOnly.FromDateTime(DateTime.Now.AddDays(-30)),
            DateOnly.FromDateTime(DateTime.Now),
            DateOnly.FromDateTime(DateTime.Now),
            660
        );
    }

    private async Task RunTestToCase(
        Guid userId,
        Guid planId,
        Plan plan,
        DateOnly startDay,
        DateOnly endDay,
        DateOnly prevDay,
        decimal checkValue)
    {
        ReturnBikeCommandHandler handler = SetupRentMock(userId, planId, plan, startDay, endDay, prevDay);

        var response = await handler.Handle(
            new ReturnBikeCommand(
                userId),
            default);

        response.TotalRentValue.Should().Be(checkValue);
    }

    private ReturnBikeCommandHandler SetupRentMock(
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

        var handler = new ReturnBikeCommandHandler(
            _userRepositoryMock.Object,
            _rentRepositoryMock.Object,
            _unitOfWorkMock.Object);
        return handler;
    }
}
