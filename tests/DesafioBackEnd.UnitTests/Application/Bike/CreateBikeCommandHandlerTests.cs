using DesafioBackEnd.Application.Bikes.Create;
using DesafioBackEnd.Domain.Contracts.Persistence;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Bike;

public class CreateBikeCommandHandlerTests
{
    private readonly Mock<IBikeRepository> _bikeRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateBikeCommandHandlerTests()
    {
        _bikeRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_SaveTheBike_WhenCallsHandle()
    {
        var command = new CreateBikeCommand(2024, "BikeType", "QXH1H1");

        _bikeRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Bike>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var handler = new CreateBikeCommandHandler(_bikeRepositoryMock.Object, _unitOfWorkMock.Object);

        await handler.Handle(command, default);

        _bikeRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Domain.Entities.Bike>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(default),
            Times.Once);
    }
}
