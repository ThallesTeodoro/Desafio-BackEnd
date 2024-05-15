namespace DesafioBackEnd.Domain.Entities;

public class Bike : BaseEntity
{
    public required Int16 Year { get; set; }
    public required string Type { get; set; }
    public required string Plate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<Rent> Rents { get; } = new List<Rent>();
}
