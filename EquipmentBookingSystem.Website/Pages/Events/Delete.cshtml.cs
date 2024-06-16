using EquipmentBookingSystem.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Website.Pages.Events;

public class DeleteModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    public DeleteModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Event Event { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eventObj = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

        if (eventObj == null)
        {
            return NotFound();
        }
        else
        {
            Event = eventObj;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var eventObj = await _context.Events.FindAsync(id);
        if (eventObj != null)
        {
            Event = eventObj;
            _context.Events.Remove(Event);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
