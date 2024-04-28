using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EquipmentBookingSystem.Website.Data;
using EquipmentBookingSystem.Website.Models;

namespace EquipmentBookingSystem.Website.Pages_Items;

public class DeleteModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    public DeleteModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Item Item { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Item.FirstOrDefaultAsync(m => m.Id == id);

        if (item == null)
        {
            return NotFound();
        }
        else
        {
            Item = item;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Item.FindAsync(id);
        if (item != null)
        {
            Item = item;
            _context.Item.Remove(Item); // TODO: Soft delete with record of history
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}