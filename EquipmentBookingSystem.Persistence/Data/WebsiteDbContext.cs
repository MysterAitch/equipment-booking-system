using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Persistence.Data;

public class WebsiteDbContext : DbContext
{
    private readonly IUserService _userService;

    public WebsiteDbContext(DbContextOptions<WebsiteDbContext> options, IUserService userService)
        : base(options)
    {
        _userService = userService;
    }

    public DbSet<Audit> Audits { get; set; } = default!;

    public DbSet<Item> Item { get; set; } = default!;

    public DbSet<Booking> Booking { get; set; } = default!;

    public DbSet<ItemIdentifier> ItemIdentifiers { get; set; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasMany(i => i.Bookings)
            .WithMany(b => b.Items);

        modelBuilder.Entity<Booking>()
            .HasMany(b => b.Items)
            .WithMany(i => i.Bookings);

        modelBuilder.Entity<ItemIdentifier>()
            .HasMany(i => i.Items)
            .WithMany(i => i.Identifiers);
    }

    public void AuditEntryForChanges()
    {
        var changedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified)
            .ToList();

        var currentUserIdentifier = _userService.GetCurrentUserEmail()
            ?? string.Join(", ", _userService.GetCurrentUserName(), _userService.GetCurrentUserId())
            ?? "unknown user";

        foreach (var changedEntity in changedEntities)
        {
            var id = changedEntity.Entity switch
            {
                Item item => item.Id,
                Booking booking => booking.Id,
                ItemIdentifier itemIdentifier => itemIdentifier.Id,
                _ => throw new Exception("unrecognised item type: " + changedEntity.Entity.GetType().Name + " in AuditEntryForChanges()")
            };

            // TODO: Also include audit entry for collection navigation properties

            var databaseValues = changedEntity.GetDatabaseValues();
            foreach (var property in changedEntity.CurrentValues.Properties)
            {
                var propertyName = property.Name;

                var currentProp = changedEntity.CurrentValues[propertyName];
                var originalProp = databaseValues[propertyName];

                // If both properties are null, move on to the next property
                if (currentProp == null && originalProp == null)
                {
                    continue;
                }

                var current = currentProp?.ToString();
                var original = originalProp?.ToString();

                // If the properties aren't both null, but are still equal, move on to the next property
                if (current == original)
                {
                    continue;
                }

                var audit = new Audit
                {
                    TableName = changedEntity.Entity.GetType().Name,
                    UserEmail = currentUserIdentifier,
                    ChangeTimeUtc = DateTime.UtcNow,
                    EntityId = id,
                    PropertyName = propertyName,
                    PropertyType = property.ClrType.Name,
                    OldValue = original,
                    NewValue = current
                };

                this.Audits.Add(audit);
            }

        }
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

        AuditEntryForChanges();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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

        AuditEntryForChanges();

        return base.SaveChangesAsync(cancellationToken);
    }
}
