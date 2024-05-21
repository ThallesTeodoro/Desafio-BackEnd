using AutoMapper;
using DesafioBackEnd.Application.Order.RegisterOrder;
using DesafioBackEnd.Domain.Contracts.EventBus;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Enums;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Order;

public class RegistarOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly Mock<IMapper> _mapperMock;

    public RegistarOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new();
        _userRepositoryMock = new();
        _unitOfWorkMock = new();
        _eventBusMock = new();
        _mapperMock = new();
    }

    [Fact]
    public async Task Handle_Should_SaveNewOrder_WhenCallMethod()
    {
        var command = new RegisterOrderCommand(50);
        var roleId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(x => x.GetAvailableDeliverymanAsync())
            .ReturnsAsync(new List<Domain.Entities.User>()
            {
                new Domain.Entities.User()
                {
                    Id = Guid.NewGuid(),
                    Email = "user@email.com",
                    Name = "User Test",
                    Role = new Domain.Entities.Role()
                    {
                        Id = roleId,
                        Name = RoleEnum.Deliveryman,
                    }
                },
                new Domain.Entities.User()
                {
                    Id = Guid.NewGuid(),
                    Email = "user2@email.com",
                    Name = "User Test 2",
                    Role = new Domain.Entities.Role()
                    {
                        Id = roleId,
                        Name = RoleEnum.Deliveryman,
                    }
                }
            });

        var expectedResponse = new OrderResponse()
        {
            Id = Guid.NewGuid(),
            Value = 50,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatusEnum.Available,
        };
        _mapperMock
            .Setup(x => x.Map<OrderResponse>(It.IsAny<Domain.Entities.Order>()))
            .Returns(expectedResponse);

        var handler = new RegisterOrderCommandHandler(
            _unitOfWorkMock.Object,
            _orderRepositoryMock.Object,
            _eventBusMock.Object,
            _mapperMock.Object,
            _userRepositoryMock.Object
        );

        var response = await handler.Handle(command, default);

        response.Should().NotBeNull();
        response.Id.Should().Be(expectedResponse.Id);
        response.Value.Should().Be(expectedResponse.Value);
        response.CreatedAt.Should().Be(expectedResponse.CreatedAt);
        response.Status.Should().Be(expectedResponse.Status);
    }
}
