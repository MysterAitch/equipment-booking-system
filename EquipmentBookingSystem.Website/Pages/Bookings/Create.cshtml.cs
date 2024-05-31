using EquipmentBookingSystem.Website.Models;
using EquipmentBookingSystem.Website.Pages.Items;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

public class CreateModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    public CreateModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        Booking = new Booking();

        // Default to a "nearby" date and time to make it easier to scroll to the correct date and time
        // (note: browsers seem to default to 0001-01-01 00:00:00)
        Booking.BookingStart = DateTime.Today;
        Booking.BookingEnd = DateTime.Today;
        Booking.EventStart = DateTime.Today;
        Booking.EventEnd = DateTime.Today;

        return Page();
    }

    [BindProperty]
    public Booking Booking { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = User.Identity?.Name ?? throw new UnidentifiedUserException();
        Booking.CreatedBy = currentUser;
        Booking.UpdatedBy = currentUser;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Booking.Add(Booking);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
