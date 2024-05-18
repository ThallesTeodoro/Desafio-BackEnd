namespace DesafioBackEnd.Domain.Enums;

public static class CnhTypeEnum
{
    public const short A = 1;
    public const short B = 2;
    public const short AB = 3;

    public static short GetCnhType(string cnhType) =>
        cnhType switch
        {
            "A" => A,
            "B" => B,
            "A+B" => AB,
            _ => 0
        };

    public static string GetCnhTypeText(short cnhType) =>
        cnhType switch
        {
            A => "A",
            B => "B",
            AB => "A+B",
            _ => ""
        };
}
