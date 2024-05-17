namespace DesafioBackEnd.Application.Common;

public static class ValidationMessages
{
    public static string RequiredField(string field) => $"The field {field} is required.";
    public static string InvalidEmailAddress(string field) => $"The field {field} is a invalid email address.";
    public static string InvalidBikeYear(int year) => $"The bike year must to be greater than {year}";
    public static string ExistenteBikePlate() => "There is already a bike with this same plate";
}
