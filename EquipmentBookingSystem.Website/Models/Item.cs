namespace EquipmentBookingSystem.Website.Models;

public class Item : BaseEntity
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;

    public List<Booking> Bookings { get; set; } = new();
}
