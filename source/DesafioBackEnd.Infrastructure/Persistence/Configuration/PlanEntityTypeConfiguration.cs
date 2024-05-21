using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioBackEnd.Infrastructure.Persistence.Configuration;

public class PlanEntityTypeConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.Property(b => b.Value)
            .HasColumnType("decimal");
    }
}
