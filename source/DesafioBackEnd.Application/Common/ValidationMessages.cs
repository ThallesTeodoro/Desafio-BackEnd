namespace DesafioBackEnd.Application.Common;

public static class ValidationMessages
{
    public static string RequiredField(string field) => $"The field {field} is required.";
    public static string InvalidEmailAddress(string field) => $"The field {field} is a invalid email address.";
}
