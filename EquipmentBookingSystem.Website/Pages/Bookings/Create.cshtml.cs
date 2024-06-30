using System.ComponentModel.DataAnnotations;
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

    public CreateModel(IBookingService bookingService, IUserService userService)
    {
        _bookingService = bookingService;
        _userService = userService;
    }

    [BindProperty]
    public DataModel Data { get; set; } = new();

    public IActionResult OnGet()
    {
        // Default to a "nearby" date and time to make it easier to scroll to the correct date and time
        // (note: browsers seem to default to 0001-01-01 00:00:00)
        Data.BookingStart = DateTime.Today;
        Data.BookingEnd = DateTime.Today;
        Data.EventStart = DateTime.Today;
        Data.EventEnd = DateTime.Today;

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
            Id = BookingId.New(),
            BookingStart = Data.BookingStart,
            BookingEnd = Data.BookingEnd,
            EventStart = Data.EventStart,
            EventEnd = Data.EventEnd,
            BookedBy = currentUser,
            BookedFor = Data.BookedFor,
            SjaEventDipsId = Data.SjaEventDipsId,
            SjaEventName = Data.SjaEventName,
            SjaEventType = Data.SjaEventType,
            Notes = Data.Notes,

        };

        var newBooking = await _bookingService.CreateNew(currentUser, booking);


        var newId = newBooking.Id?.Value;
        if (newId is null || newId == Guid.Empty)
        {
            throw new InvalidOperationException("Item ID is null");
        }

        return RedirectToPage("./Edit", new { id = newId });
    }



    public class DataModel
    {
        public DateTime EventStart { get; set; }

        public DateTime EventEnd { get; set; }

        public DateTime BookingStart { get; set; }

        public DateTime BookingEnd { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BookedFor { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SjaEventDipsId { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SjaEventName { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SjaEventType { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Notes { get; set; } = string.Empty;
    }
}
