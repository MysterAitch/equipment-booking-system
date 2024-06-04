using EquipmentBookingSystem.Website.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

public class IndexModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    public IndexModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
    }

    public IList<Booking> Bookings { get; set; } = default!;
    public IList<Item> Items { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Bookings = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .ToListAsync();
    }
}
