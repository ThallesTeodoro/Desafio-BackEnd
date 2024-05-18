namespace DesafioBackEnd.Application.Common;

public static class ValidationMessages
{
    public static string ExistenteBikePlate() => "There is already a bike with this same plate.";
    public static string ExistenteDeliverymanCnpj() => "There is already a deliveryman with this same cnpj.";
    public static string ExistenteDeliverymanCnh() => "There is already a deliveryman with this same cnh number.";
    public static string ExistenteUserEmail() => "There is already a user with this same email.";
    public static string InvalidCnhType() => "The CNH Type is invalid. Valid values: 'A', 'B', 'A+B'.";
}
