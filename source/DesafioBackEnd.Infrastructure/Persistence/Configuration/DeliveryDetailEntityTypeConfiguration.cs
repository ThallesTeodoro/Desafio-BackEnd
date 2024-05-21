using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioBackEnd.Infrastructure.Persistence.Configuration;

public class DeliveryDetailEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryDetail>
{
    public void Configure(EntityTypeBuilder<DeliveryDetail> builder)
    {
        builder.Property(b => b.Cnpj)
            .HasColumnType("varchar(14)");

        builder.Property(b => b.Cnh)
            .HasColumnType("varchar(9)");

        builder.Property(b => b.CnhImageName)
            .HasColumnType("varchar(50)");
    }
}
