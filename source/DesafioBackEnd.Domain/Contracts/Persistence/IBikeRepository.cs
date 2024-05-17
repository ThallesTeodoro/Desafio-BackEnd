using DesafioBackEnd.Domain.Dtos;
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

    /// <summary>
    /// Check if bike plate is unique
    /// </summary>
    /// <param name="plate"></param>
    /// <param name="excludeId"></param>
    /// <returns>bool</returns>
    Task<bool> BikePlateIsUniqueAsync(string plate, Guid excludeId);

    /// <summary>
    /// List bikes paginated
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="plate"></param>
    /// <returns>PaginationDto<Bike></returns>
    Task<PaginationDto<Bike>> ListPaginatedAsync(int page, int pageSize, string? plate);
}
