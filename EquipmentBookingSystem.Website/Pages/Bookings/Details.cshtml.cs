using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;

    public Booking? Booking { get; set; }

    public List<RecordChangeEntry> Changes { get; set; } = new();


    public DetailsModel(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var bookingId = new Booking.BookingId(id.Value);

        var booking = await _bookingService.GetById(bookingId);
        if (booking == null)
        {
            return NotFound();
        }

        Booking = booking;

        var changes = await _bookingService.ChangesForBooking(bookingId);
        Changes = changes.ToList();

        return Page();
    }
}
