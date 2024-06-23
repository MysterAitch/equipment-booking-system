using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Persistence.Models;

public class Booking : BaseEntity
{
    public Guid Id { get; init; }

    public HashSet<Item> Items { get; } = new();

    public DateTime BookingStart { get; set; }

    public DateTime EventStart { get; set; }

    public DateTime EventEnd { get; set; }

    public DateTime BookingEnd { get; set; }

    public string BookedBy { get; set; } = string.Empty;

    public string BookedFor { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    public string SjaEventDipsId { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    public string SjaEventName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    public string SjaEventType { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = true)]
    public string Notes { get; set; } = string.Empty;



    public EquipmentBookingSystem.Domain.Models.Booking ToDomain()
    {
        var domainBooking = new EquipmentBookingSystem.Domain.Models.Booking()
        {
            Id = new EquipmentBookingSystem.Domain.Models.Booking.BookingId(Id),
            BookingStart = BookingStart,
            BookingEnd = BookingEnd,
            EventStart = EventStart,
            EventEnd = EventEnd,
            Notes = Notes,
            // TODO: IsCancelled = IsCancelled,
            // TODO: BookedBy = BookedBy,
            BookedFor = BookedFor,
            SjaEventDipsId = SjaEventDipsId,
            SjaEventName = SjaEventName,
            SjaEventType = SjaEventType,


            // // TODO: convert separately then add on
            // Items = Items.Select(item => item.ToDomain()).ToHashSet(),

        };

        return domainBooking;
    }

    public static EquipmentBookingSystem.Persistence.Models.Booking FromDomain(Domain.Models.Booking booking)
    {
        var bookingEntity = new EquipmentBookingSystem.Persistence.Models.Booking()
        {
            Id = booking.Id?.Value ?? new Guid(),
            BookingStart = booking.BookingStart,
            BookingEnd = booking.BookingEnd,
            EventStart = booking.EventStart,
            EventEnd = booking.EventEnd,
            Notes = booking.Notes,
        };

        // bookingEntity.Items = booking.Items
        //     .Select(item => Item.FromDomain(item))
        //     .ToHashSet();

        return bookingEntity;
    }
}
