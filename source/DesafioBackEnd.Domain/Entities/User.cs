namespace DesafioBackEnd.Domain.Entities;

public class User : BaseEntity
{
    public Guid RoleId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public required Role Role { get; set; }
}
