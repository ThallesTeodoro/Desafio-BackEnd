using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioBackEnd.Infrastructure.Persistence.Configuration;

public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(b => new { b.OrderId, b.UserId });
    }
}
