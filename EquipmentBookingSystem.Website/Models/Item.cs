namespace EquipmentBookingSystem.Website.Models;

public class Item : BaseEntity
{
    public Guid Id { get; init; }

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public HashSet<Booking> Bookings { get; set; } = new();

    public HashSet<ItemIdentifier> Identifiers { get; set; } = new();


    public ItemIdentifier? SerialNumber => Identifiers.FirstOrDefault(i => i.Type == "Serial Number") ?? null;

    public ItemIdentifier? ProCloudAssetId => Identifiers.FirstOrDefault(i => i.Type == "ProCloud Asset ID") ?? null;

    public ItemIdentifier? CallSign => Identifiers.FirstOrDefault(i => i.Type == "Call Sign") ?? null;

    public ItemIdentifier? Issi => Identifiers.FirstOrDefault(i => i.Type == "ISSI") ?? null;


    public virtual string DisplayName()
    {
        return $"{CallSign?.Value} / {Issi?.Value} ({SerialNumber?.Value})";
    }
}
