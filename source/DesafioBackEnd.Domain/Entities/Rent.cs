namespace DesafioBackEnd.Domain.Entities;
using DesafioBackEnd.Domain.Extensions;

public class Rent : BaseEntity
{
    public Guid BikeId { get; set; }
    public Guid PlanId { get; set; }
    public Guid UserId { get; set; }
    public DateOnly StartDay { get; set; }
    public DateOnly EndDay { get; set; }
    public DateOnly PrevDay { get; set; }
    public short Status { get; set; }
    public decimal? TotalRentValue { get; set; }

    public Bike Bike { get; set; }
    public Plan Plan { get; set; }
    public User User { get; set; }

    public decimal CalculateTotalToPay(DateOnly returnRentDay)
    {
        decimal total;

        if (returnRentDay == this.EndDay)
        {
            total = this.Plan.Days * this.Plan.Value;
        }
        else if (returnRentDay > this.EndDay)
        {
            var diffInDays = (decimal)((returnRentDay.ConvertToDateTime() - this.EndDay.ConvertToDateTime()).TotalDays * 50);
            total = (this.Plan.Days * this.Plan.Value) + diffInDays;
        }
        else
        {
            var days = (this.EndDay.ConvertToDateTime() - returnRentDay.ConvertToDateTime()).TotalDays;
            decimal unpaidDailyValue = (decimal)days * this.Plan.Value;

            var dailyDays = (returnRentDay.ConvertToDateTime() - this.StartDay.ConvertToDateTime()).TotalDays;
            var dailyValue = (decimal)dailyDays * this.Plan.Value;

            decimal finePercent = this.Plan.FinePercent;
            var percent = finePercent / 100 * unpaidDailyValue;

            total = dailyValue + percent;
        }

        return total;
    }
}
