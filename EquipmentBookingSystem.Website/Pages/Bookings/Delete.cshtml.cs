using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;

    public Booking Booking { get; set; } = default!;

    public DeleteModel(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null || id.Value == Guid.Empty)
        {
            return BadRequest();
        }

        var bookingId = new BookingId(id.Value);

        var booking = await _bookingService.GetById(bookingId);
        if (booking == null)
        {
            return NotFound();
        }

        Booking = booking;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null || id.Value == Guid.Empty)
        {
            return BadRequest();
        }

        // TODO: Access control

        var bookingId = new BookingId(id.Value);
        await _bookingService.Delete(bookingId);

        return RedirectToPage("./Index");
    }
}
