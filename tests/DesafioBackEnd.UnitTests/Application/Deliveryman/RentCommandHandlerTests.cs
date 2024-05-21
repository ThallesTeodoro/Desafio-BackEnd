using Bogus.DataSets;
using DesafioBackEnd.Application.Deliveryman.Rent;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Enums;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Deliveryman;

public class RentCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPlanRepository> _planRepositoryMock;
    private readonly Mock<IBikeRepository> _bikeRepositoryMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public RentCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _planRepositoryMock = new();
        _bikeRepositoryMock = new();
        _rentRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async Task Handle_Should_ReturnForbiddenException_WhenCnhTypeIsInvalidToRent()
    {
        var command = new RentCommand(Guid.NewGuid(), DateTime.Now, DateTime.Now);

        _userRepositoryMock
            .Setup(x => x.FindWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.User()
            {
                Id = Guid.NewGuid(),
                Email = "email@email.com",
                Name = "User Name",
                DeliveryDetail = new Domain.Entities.DeliveryDetail()
                {
                    Id = Guid.NewGuid(),
                    Cnh = "999999999",
                    CnhImageName = "image.jpg",
                    CnhType = CnhTypeEnum.B,
                    Cnpj = "00000000000"
                }
            });

        var handler = new RentCommandHandler(
            _unitOfWorkMock.Object,
            _bikeRepositoryMock.Object,
            _planRepositoryMock.Object,
            _userRepositoryMock.Object,
            _rentRepositoryMock.Object
        );

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnForbiddenException_WhenUserIsNotAbleToRent()
    {
        var command = new RentCommand(Guid.NewGuid(), DateTime.Now, DateTime.Now);

        _userRepositoryMock
            .Setup(x => x.FindWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.User()
            {
                Id = Guid.NewGuid(),
                Email = "email@email.com",
                Name = "User Name",
                DeliveryDetail = new Domain.Entities.DeliveryDetail()
                {
                    Id = Guid.NewGuid(),
                    Cnh = "999999999",
                    CnhImageName = "image.jpg",
                    CnhType = CnhTypeEnum.A,
                    Cnpj = "00000000000"
                }
            });

        _rentRepositoryMock
            .Setup(x => x.UserIsAbleToRentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        var handler = new RentCommandHandler(
            _unitOfWorkMock.Object,
            _bikeRepositoryMock.Object,
            _planRepositoryMock.Object,
            _userRepositoryMock.Object,
            _rentRepositoryMock.Object
        );

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationException_WhenThereIsNoBikeToRent()
    {
        var command = new RentCommand(Guid.NewGuid(), DateTime.Now, DateTime.Now);

        _userRepositoryMock
            .Setup(x => x.FindWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.User()
            {
                Id = Guid.NewGuid(),
                Email = "email@email.com",
                Name = "User Name",
                DeliveryDetail = new Domain.Entities.DeliveryDetail()
                {
                    Id = Guid.NewGuid(),
                    Cnh = "999999999",
                    CnhImageName = "image.jpg",
                    CnhType = CnhTypeEnum.A,
                    Cnpj = "00000000000"
                }
            });

        _rentRepositoryMock
            .Setup(x => x.UserIsAbleToRentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _bikeRepositoryMock
            .Setup(x => x.FindAvailableBikeToRentAsync())
            .ReturnsAsync((Domain.Entities.Bike?)null);

        var handler = new RentCommandHandler(
            _unitOfWorkMock.Object,
            _bikeRepositoryMock.Object,
            _planRepositoryMock.Object,
            _userRepositoryMock.Object,
            _rentRepositoryMock.Object
        );

        Func<Task> act = handler.Awaiting(x => x.Handle(command, default));
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_Should_SaveRent_WhenCallHandle()
    {
        var command = new RentCommand(Guid.NewGuid(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(6));

        _userRepositoryMock
            .Setup(x => x.FindWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Domain.Entities.User()
            {
                Id = Guid.NewGuid(),
                Email = "email@email.com",
                Name = "User Name",
                DeliveryDetail = new Domain.Entities.DeliveryDetail()
                {
                    Id = Guid.NewGuid(),
                    Cnh = "999999999",
                    CnhImageName = "image.jpg",
                    CnhType = CnhTypeEnum.A,
                    Cnpj = "00000000000"
                }
            });

        _rentRepositoryMock
            .Setup(x => x.UserIsAbleToRentAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _bikeRepositoryMock
            .Setup(x => x.FindAvailableBikeToRentAsync())
            .ReturnsAsync(new Domain.Entities.Bike()
            {
                Id = Guid.NewGuid(),
                Plate = "QXH1H2",
                Type = "ASD",
                Year = 2020,
            });

        _planRepositoryMock
            .Setup(x => x.GetPlanByDaysAsync(It.IsAny<int>()))
            .ReturnsAsync(new Domain.Entities.Plan()
            {
                Id = Guid.NewGuid(),
                Days = 7,
                Value = 30,
                FinePercent = 20,
            });

        var handler = new RentCommandHandler(
            _unitOfWorkMock.Object,
            _bikeRepositoryMock.Object,
            _planRepositoryMock.Object,
            _userRepositoryMock.Object,
            _rentRepositoryMock.Object
        );

        await handler.Handle(command, default);

        _rentRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Domain.Entities.Rent>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(default),
            Times.Once);
    }
}
