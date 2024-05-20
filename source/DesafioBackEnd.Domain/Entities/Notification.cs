namespace DesafioBackEnd.Domain.Entities;

public class Notification
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }

    public User User { get; set; }
    public Order Order { get; set; }
}
