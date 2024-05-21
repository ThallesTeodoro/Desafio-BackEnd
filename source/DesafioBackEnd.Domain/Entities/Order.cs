namespace DesafioBackEnd.Domain.Entities;

public class Order : BaseEntity
{
    public Guid? UserId { get; set; }
    public required decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public required short Status { get; set; }

    public List<Notification> Notifications { get; set; } = new List<Notification>();
    public User User { get; set; }
}
