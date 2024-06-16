using EquipmentBookingSystem.Website.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Website.Pages.Events;

public class IndexModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    public IndexModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
    }

    public IList<Event> Event { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Event = await _context.Events.ToListAsync();
    }
}