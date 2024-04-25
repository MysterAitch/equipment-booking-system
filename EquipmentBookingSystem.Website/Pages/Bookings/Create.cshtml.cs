using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EquipmentBookingSystem.Website.Data;
using EquipmentBookingSystem.Website.Models;

namespace EquipmentBookingSystem.Website.Pages_Bookings
{
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
            Booking.CreatedBy = User.Identity?.Name;
            Booking.UpdatedBy = User.Identity?.Name;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Booking.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
