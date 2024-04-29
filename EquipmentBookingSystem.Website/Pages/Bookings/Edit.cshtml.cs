using EquipmentBookingSystem.Website.Models;
using EquipmentBookingSystem.Website.Pages.Items;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

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
    public List<CheckBoxListItem> Options { get; set; } = new();

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

        Booking = booking;

        Items = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .ToListAsync();
        Options = Items.Select(s => new CheckBoxListItem()
        {
            Id = s.Id,
            Display = s.Name,
            IsChecked = Booking.Items.Select(x => x.Id).Contains(s.Id) ? true : false
        }).ToList();

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(List<Guid> selectedOptions)
    {
        Items = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .ToListAsync();

        if (!ModelState.IsValid)
        {
            Options = Items.Select(s => new CheckBoxListItem()
            {
                Id = s.Id,
                Display = s.Name,
                IsChecked = Booking.Items.Select(x => x.Id).Contains(s.Id) ? true : false
            }).ToList();

            return Page();
        }

        var oldBooking = await _context.Booking
            .Include(b => b.Items)
            .ThenInclude(i => i.Identifiers)
            .SingleOrDefaultAsync(m => m.Id == Booking.Id);
        if (oldBooking == null)
        {
            // Attempting to edit a booking that doesn't exist
            return NotFound();
        }

        oldBooking.BookingStart = Booking.BookingStart;
        oldBooking.EventStart = Booking.EventStart;
        oldBooking.EventEnd = Booking.EventEnd;
        oldBooking.BookingEnd = Booking.BookingEnd;
        oldBooking.BookedFor = Booking.BookedFor;
        oldBooking.Notes = Booking.Notes;

        oldBooking.Items.Clear();
        // oldBooking.Items.RemoveWhere(i => Options.Any(o => o.Id == i.Id && !o.IsChecked));
        Items.Where(i => Options.Any(o => o.Id == i.Id && o.IsChecked))
            .ToList()
            .ForEach(i => oldBooking.Items.Add(i));

        oldBooking.UpdatedDate = DateTime.Now;

        var currentUser = User.Identity?.Name ?? throw new UnidentifiedUserException();
        oldBooking.UpdatedBy = currentUser;

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

        return RedirectToPage("./Details", new { id = Booking.Id });
    }

    private bool BookingExists(Guid id)
    {
        return _context.Booking.Any(e => e.Id == id);
    }


}
