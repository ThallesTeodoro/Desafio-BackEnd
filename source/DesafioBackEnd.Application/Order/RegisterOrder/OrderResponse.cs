namespace DesafioBackEnd.Application.Order.RegisterOrder;

public class OrderResponse
{
    public Guid Id { get; set; }
    public required decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public required short Status { get; set; }
}