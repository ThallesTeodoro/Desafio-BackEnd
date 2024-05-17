namespace DesafioBackEnd.Domain.Entities;

public class Plan : BaseEntity
{
    public required short Days { get; set; }
    public required decimal Value { get; set; }

    public List<Rent> Rents { get; } = new List<Rent>();
}
