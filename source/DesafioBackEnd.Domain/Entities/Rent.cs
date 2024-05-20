namespace DesafioBackEnd.Domain.Entities;

public class Rent : BaseEntity
{
    public Guid BikeId { get; set; }
    public Guid PlanId { get; set; }
    public Guid UserId { get; set; }
    public DateOnly StartDay { get; set; }
    public DateOnly EndDay { get; set; }
    public DateOnly PrevDay { get; set; }
    public short Status { get; set; }

    public Bike Bike { get; set; }
    public Plan Plan { get; set; }
    public User User { get; set; }
}
