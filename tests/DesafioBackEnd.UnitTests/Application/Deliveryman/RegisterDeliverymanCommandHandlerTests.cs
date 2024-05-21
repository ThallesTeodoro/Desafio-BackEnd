using System.Text.RegularExpressions;
using DesafioBackEnd.Application.Deliveryman.Register;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Contracts.Services;
using DesafioBackEnd.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.Deliveryman;

public class RegisterDeliverymanCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IDeliverymanRepository> _deliverymanRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IStorageService> _storageServiceMock;

    public RegisterDeliverymanCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _roleRepositoryMock = new();
        _deliverymanRepositoryMock = new();
        _unitOfWorkMock = new();
        _configurationMock = new();
        _storageServiceMock = new();
    }

    [Fact]
    public async Task Handle_Should_SaveDeliveryman_WhenCallHandle()
    {
        var command = new RegisterDeliverymanCommand(
            "Deliveryman Name",
            "deliveryman@email.com",
            "10.000.000/0001-00",
            new DateOnly(1999, 1, 1),
            "999999990",
            "A+B",
            CreateImage());

        var baseImageUrl = "http://127.0.0.1:10000/devstoreaccount1/desafiobackend";
        var imageName = "cnh/filename.jpg";

        _storageServiceMock
            .Setup(x => x.UploadFileAsync(
                It.IsAny<Stream>(),
                "cnh",
                Path.GetExtension(command.CnhImage.FileName),
                "image/jpeg"
            ))
            .ReturnsAsync(imageName)
            .Verifiable();

        _configurationMock
            .Setup(x => x["AzureBlobStorage:BaseUri"])
            .Returns(baseImageUrl);

        _roleRepositoryMock
            .Setup(x => x.AllAsync())
            .ReturnsAsync(new List<Domain.Entities.Role>()
            {
                new Domain.Entities.Role()
                {
                    Id = Guid.NewGuid(),
                    Name = RoleEnum.Deliveryman,
                },
                new Domain.Entities.Role()
                {
                    Id = Guid.NewGuid(),
                    Name = RoleEnum.Administrator,
                },
            })
            .Verifiable();

        var handler = new RegisterDeliverymanCommandHandler(
            _userRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _configurationMock.Object,
            _storageServiceMock.Object,
            _deliverymanRepositoryMock.Object);

        var response = await handler.Handle(command, default);

        response.Name.Should().Be(command.Name);
        response.Email.Should().Be(command.Email);
        response.Cnpj.Should().Be(Regex.Replace(command.Cnpj, @"[^\d]", string.Empty));
        response.Birthdate.Should().Be(command.Birthdate);
        response.Cnh.Should().Be(command.Cnh);
        response.CnhType.Should().Be(CnhTypeEnum.AB);
        response.CnhImageName.Should().Be($"{baseImageUrl}/{imageName}");

        _userRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Domain.Entities.User>()),
            Times.Once);

        _deliverymanRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Domain.Entities.DeliveryDetail>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(default),
            Times.Once);
    }

    private IFormFile CreateImage()
    {
         //Setup mock file using a memory stream
        var fileName = "test.jpg";
        var stream = new MemoryStream();
        stream.Position = 0;

        //create FormFile with desired data
        return new FormFile(stream, 0, stream.Length, "id_from_form", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg"
        };
    }
}
