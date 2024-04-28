using Microsoft.EntityFrameworkCore;
using EquipmentBookingSystem.Website.Models;

namespace EquipmentBookingSystem.Website.Data;

public class WebsiteDbContext : DbContext
{
    public WebsiteDbContext (DbContextOptions<WebsiteDbContext> options)
        : base(options)
    {
    }

    public DbSet<EquipmentBookingSystem.Website.Models.Item> Item { get; set; } = default!;

    public DbSet<EquipmentBookingSystem.Website.Models.Booking> Booking { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasMany(i => i.Bookings)
            .WithMany(b => b.Items);

        modelBuilder.Entity<Booking>()
            .HasMany(b => b.Items)
            .WithMany(i => i.Bookings);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }
}
