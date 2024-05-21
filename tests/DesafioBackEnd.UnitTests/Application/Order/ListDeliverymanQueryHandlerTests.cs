using AutoMapper;
using Bogus;
using Bogus.DataSets;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Application.Order.ListDeliverymen;
using DesafioBackEnd.Application.Order.RegisterOrder;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Order;

public class ListDeliverymanQueryHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IMapper> _mapperMock;

    public ListDeliverymanQueryHandlerTests()
    {
        _orderRepositoryMock = new();
        _configurationMock = new();
        _mapperMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFoundException_WhenOrderIsNotFound()
    {
        var command = new ListDeliverymanQuery(Guid.NewGuid());

        _orderRepositoryMock
            .Setup(x => x.FindOrderByIdWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Order?)null);

        var handler = new ListDeliverymanQueryHandler(_mapperMock.Object, _orderRepositoryMock.Object, _configurationMock.Object);

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnTheORder_WhenOrderIsFound()
    {
        var orderId = Guid.NewGuid();
        var command = new ListDeliverymanQuery(orderId);

        _orderRepositoryMock
            .Setup(x => x.FindOrderByIdWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.Order()
            {
                Id = orderId,
                Value = 50,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatusEnum.Available,
                Notifications = GetNotifications(orderId)
            });

        _mapperMock
            .Setup(x => x.Map<OrderResponse>(It.IsAny<Domain.Entities.Order>()))
            .Returns(new OrderResponse()
            {
                Id = orderId,
                Value = 50,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatusEnum.Available,
            });

        var baseImageUrl = "http://127.0.0.1:10000/devstoreaccount1/desafiobackend";
        _configurationMock
            .Setup(x => x["AzureBlobStorage:BaseUri"])
            .Returns(baseImageUrl);

        var handler = new ListDeliverymanQueryHandler(_mapperMock.Object, _orderRepositoryMock.Object, _configurationMock.Object);

        var response = await handler.Handle(command, default);

        response.Should().NotBeNull();
        response.Order.Id.Should().Be(orderId);
    }

    private static List<Notification> GetNotifications(Guid orderId)
    {
        var roleId = Guid.NewGuid();
        Random random = new Random(100);
        var users = new Faker<Domain.Entities.User>()
            .RuleFor(x => x.Id, Guid.NewGuid())
            .RuleFor(x => x.RoleId, roleId)
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.DeliveryDetail, new Faker<DeliveryDetail>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.Cnpj, "10000000000100")
                .RuleFor(x => x.Birthdate, new DateOnly(1999, 1, 1))
                .RuleFor(x => x.Cnh, "999999990")
                .RuleFor(x => x.CnhType, CnhTypeEnum.AB)
                .RuleFor(x => x.CnhImageName, $"image-{random.Next()}.jpg")
            )
            .Generate(10);

        return users
            .Select(u => new Notification()
            {
                OrderId = orderId,
                UserId = u.Id,
                User = u
            })
            .ToList();
    }
}
