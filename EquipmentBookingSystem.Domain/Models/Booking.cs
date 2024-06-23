using System;
using System.Collections.Generic;

namespace EquipmentBookingSystem.Domain.Models;

public class Booking
{
    public BookingId Id { get; init; } = default!;
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnd { get; set; }
    public DateTime EventStart { get; set; }
    public DateTime EventEnd { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }

    public HashSet<Item> Items { get; set; } = new();

    public User BookedBy { get; set; } = default!;
    public string? BookedFor { get; set; } = default!;

    public string SjaEventDipsId { get; set; } = string.Empty;

    public string SjaEventName { get; set; } = string.Empty;

    public string SjaEventType { get; set; } = string.Empty;

    public RecordMetaData RecordMetaData { get; set; } = default!;

    public virtual string DisplayName()
    {
        // return $"{BookingStart} - {BookingEnd} ({Id})";
        return $"{SjaEventName} ({BookingStart} - {BookingEnd})";
    }

    public record BookingId(Guid Value);

}
