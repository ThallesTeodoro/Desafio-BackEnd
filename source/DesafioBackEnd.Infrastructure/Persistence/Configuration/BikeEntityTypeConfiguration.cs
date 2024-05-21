using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioBackEnd.Infrastructure.Persistence.Configuration;

public class BikeEntityTypeConfiguration : IEntityTypeConfiguration<Bike>
{
    public void Configure(EntityTypeBuilder<Bike> builder)
    {
        builder.Property(b => b.Type)
            .HasColumnType("varchar(100)");

        builder.Property(b => b.Plate)
            .HasColumnType("varchar(7)");
    }
}
