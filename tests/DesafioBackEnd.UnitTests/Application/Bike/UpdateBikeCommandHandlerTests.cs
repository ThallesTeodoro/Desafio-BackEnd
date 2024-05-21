using AutoMapper;
using DesafioBackEnd.Application.Bikes.List;
using DesafioBackEnd.Application.Bikes.Update;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Bike;

public class UpdateBikeCommandHandlerTests
{
    private readonly Mock<IBikeRepository> _bikeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public UpdateBikeCommandHandlerTests()
    {
        _bikeRepositoryMock = new();
        _unitOfWorkMock = new();
        _mapperMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFoundException_WhenBikeWasNotFound()
    {
        var command = new UpdateBikeCommand(Guid.NewGuid(), "QXH1H1");

        _bikeRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult((Domain.Entities.Bike?)null))
            .Verifiable();

        var handler = new UpdateBikeCommandHandler(_mapperMock.Object, _unitOfWorkMock.Object, _bikeRepositoryMock.Object);

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationException_WhenBikePlateIsNotUnique()
    {
        var command = new UpdateBikeCommand(Guid.NewGuid(), "QXH1H1");

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

        _bikeRepositoryMock
            .Setup(x => x.BikePlateIsUniqueAsync(It.IsAny<string>(), It.IsAny<Guid>()))
            .Returns(Task.FromResult(false))
            .Verifiable();

        var handler = new UpdateBikeCommandHandler(_mapperMock.Object, _unitOfWorkMock.Object, _bikeRepositoryMock.Object);

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_Should_UpdateBike_WhenBikePlateIsUnique()
    {
        var bikeId = Guid.NewGuid();
        var newPlate = "QXH1H1";
        var command = new UpdateBikeCommand(Guid.NewGuid(), newPlate);

        _bikeRepositoryMock
            .Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.Bike()
            {
                Id = bikeId,
                Plate = "QXH1H2",
                Type = "ASD",
                Year = 2020,
            })
            .Verifiable();

        _bikeRepositoryMock
            .Setup(x => x.BikePlateIsUniqueAsync(It.IsAny<string>(), It.IsAny<Guid>()))
            .Returns(Task.FromResult(true))
            .Verifiable();

        _bikeRepositoryMock
            .Setup(x => x.Update(It.IsAny<Domain.Entities.Bike>()))
            .Verifiable();

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mapperMock
            .Setup(x => x.Map<BikeResponse>(It.IsAny<Domain.Entities.Bike>()))
            .Returns(new BikeResponse()
            {
                Id = bikeId,
                Plate = newPlate,
                Type = "ASD",
                Year = 2020,
            })
            .Verifiable();

        var handler = new UpdateBikeCommandHandler(_mapperMock.Object, _unitOfWorkMock.Object, _bikeRepositoryMock.Object);

        var response = await handler.Handle(command, default);
        response.Should().NotBeNull();
        response.Id.Should().Be(bikeId);
        response.Plate.Should().Be(newPlate);

        _bikeRepositoryMock.Verify(
            x => x.Update(It.IsAny<Domain.Entities.Bike>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(default),
            Times.Once);
    }
}
