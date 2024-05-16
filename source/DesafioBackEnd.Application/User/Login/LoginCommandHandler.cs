using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Authentication;
using DesafioBackEnd.Domain.Contracts.Persistence;
using MediatR;

namespace DesafioBackEnd.Application.User.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new NotFoundException("User was not found");
        }

        string token = _jwtProvider.Generate(user);

        return new LoginResponse("Bearer", token);
    }
}
