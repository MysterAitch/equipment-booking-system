using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Website.Models;

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

}
