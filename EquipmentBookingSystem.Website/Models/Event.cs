namespace EquipmentBookingSystem.Website.Models;

public class Event
{
    public Guid Id { get; init; }

    public DateTimeOffset EventCoverStart { get; set; }
    public DateTimeOffset EventCoverEnd { get; set; }

    public DateTime EventStart { get; set; }
    public DateTime EventEnd { get; set; }

    public string? DipsEventLocationString { get; set; } = string.Empty;
    public string? DipsEventPostcode { get; set; } = string.Empty;

    public string? DipsEventType { get; set; }
    public bool DipsIsDeleted { get; set; } = false;

    public string? DipsId { get; set; } = string.Empty;
    public string? DipsEventTitle { get; set; }

    public string? DipsAllocatedArea { get; set; }
    public string? DipsAllocatedDistrict { get; set; }
}
