using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Website.Pages.Shared;
using EquipmentBookingSystem.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

public class EditModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;

    public EditModel(IBookingService bookingService, IUserService userService)
    {
        _bookingService = bookingService;
        _userService = userService;
    }

    [BindProperty]
    public Guid BookingId { get; set; }

    [BindProperty]
    public DateTime EventStart { get; set; }

    [BindProperty]
    public DateTime EventEnd { get; set; }

    [BindProperty]
    public DateTime BookingStart { get; set; }

    [BindProperty]
    public DateTime BookingEnd { get; set; }

    [BindProperty]
    public string BookedFor { get; set; } = string.Empty;

    [BindProperty]
    public string SjaEventDipsId { get; set; } = string.Empty;

    [BindProperty]
    public string SjaEventName { get; set; } = string.Empty;

    [BindProperty]
    public string SjaEventType { get; set; } = string.Empty;

    [BindProperty]
    public string Notes { get; set; } = string.Empty;


    [BindProperty]
    public List<CheckBoxListItem> Options { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null || id.Value == Guid.Empty)
        {
            return BadRequest();
        }

        var bookingId = new BookingId(id.Value);
        var booking = await _bookingService.GetById(bookingId);
        if (booking == null)
        {
            return NotFound();
        }

        BookingId = bookingId.Value;
        BookingStart = booking.BookingStart;
        EventStart = booking.EventStart;
        EventEnd = booking.EventEnd;
        BookingEnd = booking.BookingEnd;
        BookedFor = booking.BookedFor ?? string.Empty;
        Notes = booking.Notes;

        SjaEventName = booking.SjaEventName;
        SjaEventDipsId = booking.SjaEventDipsId;
        SjaEventType = booking.SjaEventType;


        var items = await _bookingService
            .ItemsPotentiallyAvailableForBooking(bookingId);
        Options = items
            .OrderBy(i => i.DisplayName())
            .Select(s => new CheckBoxListItem()
            {
                Id = s.Id?.Value ?? throw new NullReferenceException(),
                Display = s.DisplayName(),
                IsChecked = booking.Items.Select(x => x.Id).Contains(s.Id) ? true : false
            })
            .ToList();

        return Page();
    }


    public async Task<IActionResult> OnPostAsync(Guid? id, List<Guid> selectedOptions)
    {
        if (id == null || id.Value == Guid.Empty)
        {
            return BadRequest();
        }

        var bookingId = new BookingId(id.Value);
        var booking = await _bookingService.GetById(bookingId);
        if (booking == null)
        {
            // Attempting to edit a booking that doesn't exist
            return NotFound();
        }

        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        booking.BookingStart = BookingStart;
        booking.EventStart = EventStart;
        booking.EventEnd = EventEnd;
        booking.BookingEnd = BookingEnd;
        booking.BookedFor = BookedFor;
        booking.Notes = Notes;

        booking.SjaEventName = SjaEventName;
        booking.SjaEventDipsId = SjaEventDipsId;
        booking.SjaEventType = SjaEventType;

        var items = await _bookingService
            .ItemsPotentiallyAvailableForBooking(bookingId);

        // TODO: Update to not require full Item objects -- just the item IDs.
        booking.Items.Clear();
        items.Where(i => Options.Any(o => o.Id == i.Id.Value.Value && o.IsChecked))
            .ToList()
            .ForEach(i => booking.Items.Add(i));


        await _bookingService.Update(bookingId, currentUser, booking);

        return RedirectToPage("./Details", new { id = booking.Id.Value });
    }
}
