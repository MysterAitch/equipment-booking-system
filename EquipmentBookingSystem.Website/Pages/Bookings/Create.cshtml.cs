using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

public class CreateModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;


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


    public CreateModel(IBookingService bookingService, IUserService userService)
    {
        _bookingService = bookingService;
        _userService = userService;
    }

    public IActionResult OnGet()
    {
        // Default to a "nearby" date and time to make it easier to scroll to the correct date and time
        // (note: browsers seem to default to 0001-01-01 00:00:00)
        BookingStart = DateTime.Today;
        BookingEnd = DateTime.Today;
        EventStart = DateTime.Today;
        EventEnd = DateTime.Today;

        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();
        var booking = new Booking()
        {
            BookingStart = BookingStart,
            BookingEnd = BookingEnd,
            EventStart = EventStart,
            EventEnd = EventEnd,
            BookedBy = currentUser,
            BookedFor = BookedFor,
            SjaEventDipsId = SjaEventDipsId,
            SjaEventName = SjaEventName,
            SjaEventType = SjaEventType,
            Notes = Notes,

        };

        var newBooking = await _bookingService.CreateNew(currentUser, booking);


        var newId = newBooking.Id?.Value;
        if (newId is null || newId == Guid.Empty)
        {
            throw new InvalidOperationException("Item ID is null");
        }

        return RedirectToPage("./Edit", new {id = newId});
    }
}
