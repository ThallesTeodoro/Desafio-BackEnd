namespace DesafioBackEnd.Domain.Entities;

public class Order : BaseEntity
{
    public Guid? UserId { get; set; }
    public required decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public required Int16 Status { get; set; }

    public List<Notification> Notifications { get; } = new List<Notification>();
}
