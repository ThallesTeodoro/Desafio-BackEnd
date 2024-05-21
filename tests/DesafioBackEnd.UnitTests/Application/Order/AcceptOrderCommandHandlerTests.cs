using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Application.Order.AcceptOrder;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Enums;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Order;

public class AcceptOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public AcceptOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFoundException_WhenOrderIsNotFound()
    {
        var command = new AcceptOrderCommand(Guid.NewGuid(), Guid.NewGuid());

        _orderRepositoryMock
            .Setup(x => x.FindOrderByIdWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Order?)null);

        var handler = new AcceptOrderCommandHandler(_unitOfWorkMock.Object, _orderRepositoryMock.Object);

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnForbiddenException_WhenOrderWasNotNotifiedToUserOrWasAccepted()
    {
        var command = new AcceptOrderCommand(Guid.NewGuid(), Guid.NewGuid());

        _orderRepositoryMock
            .Setup(x => x.FindOrderByIdWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.Order()
            {
                Id = Guid.NewGuid(),
                Value = 50,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatusEnum.Accepted,
                Notifications = new List<Domain.Entities.Notification>(),
            });

        var handler = new AcceptOrderCommandHandler(_unitOfWorkMock.Object, _orderRepositoryMock.Object);

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Handle_Should_AcceptTheOrder_WhenOrderWasNotifiedToUserAndOrderWasNotAccepted()
    {
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var command = new AcceptOrderCommand(orderId, userId);

        _orderRepositoryMock
            .Setup(x => x.FindOrderByIdWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.Order()
            {
                Id = orderId,
                Value = 50,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatusEnum.Available,
                Notifications = new List<Domain.Entities.Notification>()
                {
                    new Domain.Entities.Notification()
                    {
                        OrderId = orderId,
                        UserId = userId,
                    }
                },
            });

        var handler = new AcceptOrderCommandHandler(_unitOfWorkMock.Object, _orderRepositoryMock.Object);

        await handler.Handle(command, default);

        _orderRepositoryMock.Verify(
            x => x.Update(It.IsAny<Domain.Entities.Order>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(default),
            Times.Once);
    }
}
