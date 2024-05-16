namespace DesafioBackEnd.Domain.Entities;

public class DeliveryDetail : BaseEntity
{
    public Guid UserId { get; set; }
    public required string Cnpj { get; set; }
    public DateOnly Birthdate { get; set; }
    public required string Cnh { get; set; }
    public required Int16 CnhType { get; set; }
    public required string CnhImageName { get; set; }

    public  required User User { get; set; }
}
