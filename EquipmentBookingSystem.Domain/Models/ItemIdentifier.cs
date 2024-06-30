using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StronglyTypedIds;

namespace EquipmentBookingSystem.Domain.Models;

public class ItemIdentifier
{
    public ItemIdentifierId? Id { get; init; } = default!;

    public string Type { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? From { get; set; }

    [DataType(DataType.Date)]
    public DateTime? To { get; set; }

    public HashSet<Item> Items { get; } = new();

}

[StronglyTypedId(Template.Guid)]
public partial struct ItemIdentifierId { }

