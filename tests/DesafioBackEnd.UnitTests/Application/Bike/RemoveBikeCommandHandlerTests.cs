using DesafioBackEnd.Application.Bikes.Remove;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Bike;

public class RemoveBikeCommandHandlerTests
{
    private readonly Mock<IBikeRepository> _bikeRepositoryMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public RemoveBikeCommandHandlerTests()
    {
        _bikeRepositoryMock = new();
        _unitOfWorkMock = new();
        _rentRepositoryMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFoundException_WhenBikeWasNotFound()
    {
        var command = new RemoveBikeCommand(Guid.NewGuid());

        _bikeRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult((Domain.Entities.Bike?)null))
            .Verifiable();

        var handler = new RemoveBikeCommandHandler(_unitOfWorkMock.Object, _bikeRepositoryMock.Object, _rentRepositoryMock.Object);

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnForbidden_WhenThereAreRentsRelated()
    {
        var command = new RemoveBikeCommand(Guid.NewGuid());

        _bikeRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.Bike()
            {
                Id = Guid.NewGuid(),
                Plate = "QXH1H2",
                Type = "ASD",
                Year = 2020,
            })
            .Verifiable();

        _rentRepositoryMock
            .Setup(x => x.CheckBikeRentAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(true))
            .Verifiable();

        var handler = new RemoveBikeCommandHandler(_unitOfWorkMock.Object, _bikeRepositoryMock.Object, _rentRepositoryMock.Object);

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Handle_Should_RemoveBike_WhenCallHandler()
    {
        var command = new RemoveBikeCommand(Guid.NewGuid());

        _bikeRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.Bike()
            {
                Id = Guid.NewGuid(),
                Plate = "QXH1H2",
                Type = "ASD",
                Year = 2020,
            })
            .Verifiable();

        _rentRepositoryMock
            .Setup(x => x.CheckBikeRentAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(false))
            .Verifiable();

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var handler = new RemoveBikeCommandHandler(_unitOfWorkMock.Object, _bikeRepositoryMock.Object, _rentRepositoryMock.Object);

        await handler.Handle(command, default);

        _bikeRepositoryMock.Verify(
            x => x.Remove(It.IsAny<Domain.Entities.Bike>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(default),
            Times.Once);
    }
}
