using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Authentication;

public interface IJwtProvider
{
    string Generate(User user);
}
