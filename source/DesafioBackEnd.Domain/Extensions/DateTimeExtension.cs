namespace DesafioBackEnd.Domain.Extensions;

public static class DateTimeExtension
{
    public static DateTime ResetTimeToStartOfDay(this DateTime dateTime)
    {
        return new DateTime(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            0,
            0,
            0,
            0
        );
    }

    public static DateTime ResetTimeToEndOfDay(this DateTime dateTime)
    {
        return new DateTime(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            23,
            59,
            59,
            999
        );
    }

    public static DateTime ConvertToDateTime(this DateOnly dateOnly)
    {
        return new DateTime(
            dateOnly.Year,
            dateOnly.Month,
            dateOnly.Day,
            0,
            0,
            0,
            0
        );
    }
}
