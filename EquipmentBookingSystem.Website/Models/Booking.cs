using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Website.Models;

public class Booking : BaseEntity
{
    public Guid Id { get; init; }

    public HashSet<Item> Items { get; } = new();

    public DateTime BookingStart { get; set; }

    public DateTime EventStart { get; set; }

    public DateTime EventEnd { get; set; }

    public DateTime BookingEnd { get; set; }

    [DataType(DataType.EmailAddress)]
    public string BookedBy { get; set; } = string.Empty;

    [DataType(DataType.EmailAddress)]
    public string BookedFor { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
}
