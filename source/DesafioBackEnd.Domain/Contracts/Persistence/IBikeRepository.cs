using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IBikeRepository : IRepository<Bike>
{
    /// <summary>
    /// Check if bike plate is unique
    /// </summary>
    /// <param name="plate"></param>
    /// <returns>bool</returns>
    Task<bool> BikePlateIsUniqueAsync(string plate);
}
