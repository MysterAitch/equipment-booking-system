using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EquipmentBookingSystem.Website.Models;
using EquipmentBookingSystem.Website.Pages_Items;

namespace EquipmentBookingSystem.Website.Pages_Bookings;

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
