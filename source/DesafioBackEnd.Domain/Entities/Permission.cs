namespace DesafioBackEnd.Domain.Entities;

public class Permission : BaseEntity
{
    public required string Name { get; set; }

    public List<RolePermission> RolePermissions { get; } = new List<RolePermission>();
}
