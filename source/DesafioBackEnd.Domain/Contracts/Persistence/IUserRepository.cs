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

    /// <summary>
    /// Get entity by id with relationship
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>User instance</returns>
    User? FindByIdWithUserRoles(Guid id);

    /// <summary>
    /// Get entity by id with relationship
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>User instance</returns>
    Task<User?> FindWithRelationshipAsync(Guid id);
}
