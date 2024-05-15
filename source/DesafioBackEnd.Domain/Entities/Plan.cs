namespace DesafioBackEnd.Domain.Entities;

public class Plan : BaseEntity
{
    public required Int16 Days { get; set; }
    public required decimal Value { get; set; }

    public List<Rent> Rents { get; } = new List<Rent>();
}
