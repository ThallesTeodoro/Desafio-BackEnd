namespace DesafioBackEnd.Domain.Entities;

public class User : BaseEntity
{
    public Guid RoleId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public Role Role { get; set; }
    public DeliveryDetail DeliveryDetail { get; set; }
    public List<Rent> Rents { get; set; } = new List<Rent>();
    public List<Order> Orders { get; set; } = new List<Order>();
}
