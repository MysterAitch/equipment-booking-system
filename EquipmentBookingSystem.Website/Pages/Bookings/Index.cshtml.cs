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
    public class IndexModel : PageModel
    {
        private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

        public IndexModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Booking = await _context.Booking.ToListAsync();
        }
    }
}