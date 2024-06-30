using System;
using System.Collections.Generic;
using System.Linq;
using EquipmentBookingSystem.Domain.Models;

namespace EquipmentBookingSystem.Persistence.Models;

public class Item : BaseEntity
{
    public Guid Id { get; init; }

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public HashSet<Booking> Bookings { get; set; } = new();

    public HashSet<ItemIdentifier> Identifiers { get; set; } = new();

    public String? DamageNotes { get; set; } = string.Empty;

    public String? Notes { get; set; } = string.Empty;


    public EquipmentBookingSystem.Domain.Models.Item ToDomain()
    {
        var domainItem = new EquipmentBookingSystem.Domain.Models.Item()
        {
            Id = new EquipmentBookingSystem.Domain.Models.ItemId(Id),
            Manufacturer = Manufacturer,
            Model = Model,
            DamageNotes = DamageNotes,
            Notes = Notes,
            RecordMetaData = new RecordMetaData()
            {
                // TODO: CreatedBy = new User(CreatedBy),
                CreatedAt = CreatedDate,
                // TODO: UpdatedBy = new User(RecordMetaData.UpdatedBy),
                UpdatedAt = UpdatedDate,
            },

            // TODO: convert separately then add on
            Identifiers = Identifiers
                .Select(identifier => identifier.ToDomain())
                .ToHashSet(),
        };

        return domainItem;
    }


    public static EquipmentBookingSystem.Persistence.Models.Item FromDomain(Domain.Models.Item item)
    {
        var itemEntity = new EquipmentBookingSystem.Persistence.Models.Item()
        {
            Id = item.Id?.Value ?? Guid.Empty,
            Manufacturer = item.Manufacturer,
            Model = item.Model,
            DamageNotes = item.DamageNotes,
            Notes = item.Notes,
            CreatedDate = item.RecordMetaData.CreatedAt,
            UpdatedDate = item.RecordMetaData.UpdatedAt,
        };

        itemEntity.Identifiers = item.Identifiers
            .Select(ItemIdentifier.FromDomain)
            .ToHashSet();

        return itemEntity;
    }
}
