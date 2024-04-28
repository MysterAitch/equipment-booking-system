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

namespace EquipmentBookingSystem.Website.Pages_Bookings
{
    public class EditModel : PageModel
    {
        private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

        public EditModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;

        protected internal List<Item> Items { get; set; } = new();

        [BindProperty]
        public List<string> SelectedItems { get; set; }

        [BindProperty]
        public List<CheckBoxListItem> Options { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking =  await _context.Booking
                .Include(b => b.Items)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            Booking = booking;

            Items = await _context.Item.ToListAsync();
            Options = Items.Select(s => new CheckBoxListItem()
            {
                ID = s.Id,
                Display = s.Name,
                IsChecked = Booking.Items.Select(x => x.Id).Contains(s.Id) ? true : false
            }).ToList();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(List<Guid> selectedOptions)
        {
            Items = await _context.Item.ToListAsync();

            if (!ModelState.IsValid)
            {
                Options = Items.Select(s => new CheckBoxListItem()
                {
                    ID = s.Id,
                    Display = s.Name,
                    IsChecked = Booking.Items.Select(x => x.Id).Contains(s.Id) ? true : false
                }).ToList();

                return Page();
            }

            var oldBooking = await _context.Booking.SingleOrDefaultAsync(m => m.Id == Booking.Id);
            oldBooking.BookingStart = Booking.BookingStart;
            oldBooking.EventStart = Booking.EventStart;
            oldBooking.EventEnd = Booking.EventEnd;
            oldBooking.BookingEnd = Booking.BookingEnd;
            oldBooking.BookedFor = Booking.BookedFor;
            oldBooking.Notes = Booking.Notes;

            oldBooking.Items.Clear();
            foreach (var item in Items)
            {
                var option = Options.SingleOrDefault(o => o.ID == item.Id);
                if (option == null)
                {
                    continue;
                }

                if (option.IsChecked)
                {
                    oldBooking.Items.Add(item);
                }
            }

            oldBooking.UpdatedDate = DateTime.Now;

            var x = User.Identity?.Name;
            oldBooking.UpdatedBy = x;


            // TODO: Record history of changes

            // _context.Attach(Booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(Booking.Id))
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

        private bool BookingExists(Guid id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }


    }

    public class CheckBoxListItem
    {
        public Guid ID { get; set; }
        public string Display { get; set; }
        public bool IsChecked { get; set; }
    }
}
