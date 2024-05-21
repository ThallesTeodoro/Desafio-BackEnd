using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioBackEnd.Infrastructure.Persistence.Configuration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(b => b.Name)
            .HasColumnType("varchar(255)");

        builder.Property(b => b.Email)
            .HasColumnType("varchar(255)");
    }
}
