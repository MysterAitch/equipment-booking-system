using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Persistence.Models;

public class ItemIdentifier
{
    public Guid Id { get; init; }

    public string Type { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? From { get; set; }

    [DataType(DataType.Date)]
    public DateTime? To { get; set; }

    public HashSet<Item> Items { get; } = new();


    public EquipmentBookingSystem.Domain.Models.ItemIdentifier ToDomain()
    {
        var domainItemIdentifier = new EquipmentBookingSystem.Domain.Models.ItemIdentifier()
        {
            Id = new EquipmentBookingSystem.Domain.Models.ItemIdentifierId(Id),
            Type = Type,
            Value = Value,
            From = From,
            To = To,
            // RecordMetaData = new RecordMetaData()
            // {
            // },
        };

        return domainItemIdentifier;
    }


    public static EquipmentBookingSystem.Persistence.Models.ItemIdentifier FromDomain(Domain.Models.ItemIdentifier identifier)
    {
        var id = identifier.Id?.Value ?? Guid.Empty;
        if (id == Guid.Empty)
        {
            id = Guid.NewGuid();
        }

        var itemIdentifierEntity = new EquipmentBookingSystem.Persistence.Models.ItemIdentifier()
        {
            Id = id,
            Type = identifier.Type,
            Value = identifier.Value,
            From = identifier.From,
            To = identifier.To,
        };

        return itemIdentifierEntity;
    }
}
