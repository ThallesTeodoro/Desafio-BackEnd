using AutoMapper;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Application.User.GetUser;
using DesafioBackEnd.Domain.Contracts.Persistence;
using DesafioBackEnd.Domain.Enums;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.User;

public class GetUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public GetUserCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _mapperMock = new();
    }

    [Fact]
    public void Handle_Should_ReturnNotFoundException_WhenUserWasNotFound()
    {
        var query = new GetUserQuery(Guid.NewGuid());

        _userRepositoryMock
            .Setup(x => x.FindWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.User?)null);

        var handler = new GetUserQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);

        Func<Task> func = async () => await handler.Handle(query, default);

        func.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnTheUser_WhenUserWasFound()
    {
        var user = new Domain.Entities.User()
        {
            Id = Guid.NewGuid(),
            Email = "user@email.com",
            Name = "User Test",
            Role = new Domain.Entities.Role()
            {
                Id = Guid.NewGuid(),
                Name = RoleEnum.Administrator,
            }
        };
        var query = new GetUserQuery(user.Id);

        _userRepositoryMock
            .Setup(x => x.FindWithRelationshipAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        var responseObject = new UserResponse()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Profile = user.Role.Name,
        };
        _mapperMock.Setup(
            x => x.Map<UserResponse>(user))
            .Returns(responseObject)
            .Verifiable();

        var handler = new GetUserQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);

        var response = await handler.Handle(query, default);
        response.Should().NotBeNull();
        response.Should().Be(responseObject);
    }
}
