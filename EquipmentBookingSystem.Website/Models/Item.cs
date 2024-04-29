namespace EquipmentBookingSystem.Website.Models;

public class Item : BaseEntity
{
    public Guid Id { get; init; }

    public string Name { get; set; } = string.Empty;

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public HashSet<Booking> Bookings { get; set; } = new();

    public HashSet<ItemIdentifier> Identifiers { get; set; } = new();

    public virtual string DisplayName()
    {
        // return $"{Name} ({Id})";
        return $"{Name}";
    }
}
