using DesafioBackEnd.Application.Deliveryman.Update;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Contracts.Services;
using DesafioBackEnd.Domain.Enums;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Deliveryman;

public class UpdateDeliverymanCommandHandlerTests
{
    private readonly Mock<IDeliverymanRepository> _deliverymanRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStorageService> _storageServiceMock;

    public UpdateDeliverymanCommandHandlerTests()
    {
        _deliverymanRepositoryMock = new();
        _unitOfWorkMock = new();
        _storageServiceMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFoundException_WhenUserIsNotFound()
    {
        var command = new UpdateDeliverymanCommand(Guid.NewGuid(), ImageHelper.CreateImage());

        _deliverymanRepositoryMock
            .Setup(x => x.FindByUserIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.DeliveryDetail?)null);

        var handler = new UpdateDeliverymanCommandHandler(
            _storageServiceMock.Object,
            _unitOfWorkMock.Object,
            _deliverymanRepositoryMock.Object
        );

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_UpdateDeliveryman_WhenUserIsFound()
    {
        var id = Guid.NewGuid();
        var command = new UpdateDeliverymanCommand(id, ImageHelper.CreateImage());

        _deliverymanRepositoryMock
            .Setup(x => x.FindByUserIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.DeliveryDetail()
            {
                Id = id,
                Cnpj = "10.000.000/0001-00",
                Birthdate = new DateOnly(1999, 1, 1),
                Cnh = "999999990",
                CnhType = CnhTypeEnum.AB,
                CnhImageName = "cnh/filename.jpg",
            });

        _storageServiceMock
            .Setup(x => x.UploadFileAsync(
                It.IsAny<Stream>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync("cnh/new-filename.jpg")
            .Verifiable();

        var handler = new UpdateDeliverymanCommandHandler(
            _storageServiceMock.Object,
            _unitOfWorkMock.Object,
            _deliverymanRepositoryMock.Object
        );

        await handler.Handle(command, default);

        _storageServiceMock.Verify(
            x => x.UploadFileAsync(
                It.IsAny<Stream>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Once);

        _storageServiceMock.Verify(
            x => x.DeleteFileIfExistsAsync(
                It.IsAny<string>(),
                default),
            Times.Once);

        _deliverymanRepositoryMock.Verify(
            x => x.Update(It.IsAny<Domain.Entities.DeliveryDetail>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(default),
            Times.Once);
    }
}
