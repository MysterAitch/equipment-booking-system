using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EquipmentBookingSystem.Website.Data;
using EquipmentBookingSystem.Website.Models;

namespace EquipmentBookingSystem.Website.Pages_Items
{
    public class EditModel : PageModel
    {
        private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

        public EditModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
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

            var item =  await _context.Item.FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            Item = item;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oldItem = await _context.Item.SingleOrDefaultAsync(m => m.Id == Item.Id);
            oldItem.Name = Item.Name;
            oldItem.UpdatedDate = DateTime.Now;

            var x = User.Identity?.Name;
            oldItem.UpdatedBy = x;


            // TODO: Record history of changes


            // _context.Attach(Item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(Item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ItemExists(Guid id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
