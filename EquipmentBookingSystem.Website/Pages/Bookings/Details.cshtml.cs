using EquipmentBookingSystem.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

public class DetailsModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    public DetailsModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
    }

    public Booking Booking { get; set; } = default!;

    public List<Audit> Changes { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var booking = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (booking == null)
        {
            return NotFound();
        }
        else
        {
            Booking = booking;
        }

        Changes = await _context.Audits
            .Where(a => a.EntityId == id)
            .Where(a => !a.PropertyName.Equals("CreatedDate"))
            .Where(a => !a.PropertyName.Equals("UpdatedDate"))
            .Where(a => !a.PropertyName.Equals("CreatedBy"))
            .Where(a => !a.PropertyName.Equals("UpdatedBy"))
            .OrderByDescending(a => a.ChangeTimeUtc)
            .ToListAsync();

        return Page();
    }
}
