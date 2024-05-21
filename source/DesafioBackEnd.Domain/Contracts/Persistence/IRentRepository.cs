using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IRentRepository : IRepository<Rent>
{
    /// <summary>
    /// Check if there are rents related to the bike
    /// </summary>
    /// <param name="bikeId"></param>
    /// <returns>bool</returns>
    Task<bool> CheckBikeRentAsync(Guid bikeId);

    /// <summary>
    /// Inform if user is able to rent
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>bool</returns>
    Task<bool> UserIsAbleToRentAsync(Guid userId);

    /// <summary>
    /// Find current user rent
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Rent | null</returns>
    Task<Rent?> FindCurrentUserRentAsync(Guid userId);
}
