namespace DesafioBackEnd.Application.Bikes.List;

public class BikeResponse
{
    public Guid Id { get; set; }
    public required short Year { get; set; }
    public required string Type { get; set; }
    public required string Plate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}