namespace DesafioBackEnd.Domain.Enums;

public static class RoleEnum
{
    public const string Administrator = "Administrator";
    public const string Deliveryman = "Deliveryman";

    public static List<string> List()
    {
        return new List<string>()
        {
            Administrator,
            Deliveryman,
        };
    }
}
