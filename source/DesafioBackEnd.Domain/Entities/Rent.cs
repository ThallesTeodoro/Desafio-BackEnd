namespace DesafioBackEnd.Domain.Entities;

public class Rent : BaseEntity
{
    public Guid BikeId { get; set; }
    public Guid PlanId { get; set; }
    public Guid UserId { get; set; }
    public DateOnly StartDay { get; set; }
    public DateOnly EndDay { get; set; }
    public DateOnly PrevDay { get; set; }

    public  required Bike Bike { get; set; }
    public  required Plan Plan { get; set; }
    public  required User User { get; set; }
}
