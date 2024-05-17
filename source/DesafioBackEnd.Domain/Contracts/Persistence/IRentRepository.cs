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
}
