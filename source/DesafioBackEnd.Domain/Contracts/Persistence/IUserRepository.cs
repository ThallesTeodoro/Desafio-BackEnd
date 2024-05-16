using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Find user by email address
    /// </summary>
    /// <param name="email"></param>
    /// <returns>User | null</returns>
    Task<User?> FindByEmailAsync(string email);

    /// <summary>
    /// Find user by email address
    /// </summary>
    /// <param name="email"></param>
    /// <returns>User | null</returns>
    User? FindByEmail(string email);
}
