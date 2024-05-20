namespace DesafioBackEnd.Domain.Enums;

public static class PermissionEnum
{
    public const string ManageBikes = "manage-bikes";
    public const string DeliverymanRegister = "deliveryman-reg";
    public const string BikeRent = "bike-rent";
    public const string RegisterOrder = "register-order";

    public static List<string> List()
    {
        return new List<string>()
        {
            ManageBikes,
            DeliverymanRegister,
            BikeRent,
            RegisterOrder,
        };
    }
}
