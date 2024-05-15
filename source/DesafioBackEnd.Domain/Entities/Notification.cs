namespace DesafioBackEnd.Domain.Entities;

public class Notification
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }

    public  required User User { get; set; }
    public  required Order Order { get; set; }
}
