using DesafioBackEnd.Domain.Entities;

namespace DesafioBackEnd.Domain.Contracts.Persistence;

public interface IDeliverymanRepository : IRepository<DeliveryDetail>
{
    /// <summary>
    /// Check if the deliveryman cnpj is unique
    /// </summary>
    /// <param name="cnpj"></param>
    /// <returns>bool</returns>
    Task<bool> CnpjIsUniqueAsync(string cnpj);

    /// <summary>
    /// Check if the deliveryman cnh is unique
    /// </summary>
    /// <param name="cnh"></param>
    /// <returns>bool</returns>
    Task<bool> CnhIsUniqueAsync(string cnh);
}
