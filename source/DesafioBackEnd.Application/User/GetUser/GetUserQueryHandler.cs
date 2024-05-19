using AutoMapper;
using DesafioBackEnd.Application.Exceptions;
using DesafioBackEnd.Domain.Contracts.Persistence;
using MediatR;

namespace DesafioBackEnd.Application.User.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindWithRelationshipAsync(request.UserId);

        if (user is null)
        {
            throw new NotFoundException("User was not found");
        }

        var mappedUser = _mapper.Map<UserResponse>(user);

        return mappedUser;
    }
}
