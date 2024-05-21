using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Application.User.Login;
using DesafioBackEnd.Domain.Contracts.Authentication;
using DesafioBackEnd.Domain.Contracts.Persistence;
using FluentAssertions;
using Moq;

namespace DesafioBackEnd.UnitTests.Application.User.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _jwtProviderMock = new();
    }

    [Fact]
    public void Handle_Should_ReturnUnauthorizedException_WhenUserWasNotFound()
    {
        var command = new LoginCommand("usernotfound@email.com");

        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.User?)null);

        var handler = new LoginCommandHandler(_userRepositoryMock.Object, _jwtProviderMock.Object);

        Func<Task> func = async () => await handler.Handle(command, default);

        func.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_Should_ReturnTheToken_WhenUserWasFound()
    {
        var email = "user@email.com";
        var command = new LoginCommand(email);
        var user = new Domain.Entities.User()
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = "User Test"
        };

        _userRepositoryMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new Domain.Entities.User()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Name = "User Test"
            });

        _jwtProviderMock
            .Setup(x => x.Generate(user))
            .Returns(RandomToken(273));

        var handler = new LoginCommandHandler(_userRepositoryMock.Object, _jwtProviderMock.Object);

        var response = await handler.Handle(command, default);
        response.Should().NotBeNull();
    }

    private string RandomToken(int length)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
