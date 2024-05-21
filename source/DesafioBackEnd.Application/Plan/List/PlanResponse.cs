namespace DesafioBackEnd.Application.Plan.List;

public class PlanResponse
{
    public Guid Id { get; set; }
    public required short Days { get; set; }
    public required decimal Value { get; set; }
}