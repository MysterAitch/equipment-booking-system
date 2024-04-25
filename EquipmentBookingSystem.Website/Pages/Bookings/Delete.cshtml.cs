using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EquipmentBookingSystem.Website.Data;
using EquipmentBookingSystem.Website.Models;

namespace EquipmentBookingSystem.Website.Pages_Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

        public DeleteModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                Booking = booking;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                Booking = booking;
                _context.Booking.Remove(Booking); // TODO: Soft delete with record of history
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
