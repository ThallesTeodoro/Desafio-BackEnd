namespace DesafioBackEnd.Application.User.GetUser;

public class UserDetailResponse
{
    public required string Cnpj { get; set; }
    public DateOnly Birthdate { get; set; }
    public required string Cnh { get; set; }
    public required short CnhType { get; set; }
    public required string CnhImageName { get; set; }
}
