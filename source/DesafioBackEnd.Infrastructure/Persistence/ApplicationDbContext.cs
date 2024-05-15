using DesafioBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Bike> Bikes { get; set; }
    public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Plan> Rent { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
