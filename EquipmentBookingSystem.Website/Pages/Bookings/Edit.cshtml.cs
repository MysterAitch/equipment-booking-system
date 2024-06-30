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
    private readonly IItemService _itemService;
    private readonly IUserService _userService;

    public EditModel(IBookingService bookingService, IItemService itemService, IUserService userService)
    {
        _bookingService = bookingService;
        _itemService = itemService;
        _userService = userService;
    }


    [BindProperty]
    public DataModel Data { get; set; } = new();


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

        Data = DataModel.FromDomain(booking);

        var items = await _bookingService
            .ItemsPotentiallyAvailableForBooking(bookingId);
        Data.Options = items
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

        if (!ModelState.IsValid)
        {
            // Redisplay the form with the submitted data
            return Page();
        }

        DataModel.UpdateBooking(booking, Data);

        var items = (await _bookingService
            .ItemsPotentiallyAvailableForBooking(bookingId)).ToList();

        var selectedItemIds = Data.Options
            .Where(o => o.IsChecked)
            .Select(o => o.Id)
            .ToList();

        var selectedButUnavailableItemIds = selectedItemIds
            .Except(items.Select(i => i.Id.Value.Value))
            .ToList();

        if (selectedButUnavailableItemIds.Any())
        {
            // Attempting to book an item that is not available
            // TODO: Show user a message -- add an error and return Page(), maybe?
            return BadRequest();
        }


        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();
        await _bookingService.Update(bookingId, currentUser, booking);

        await _bookingService.UpdateItems(bookingId, selectedItemIds);

        return RedirectToPage("./Details", new { id = booking.Id });
    }



    public class DataModel
    {
        public Guid BookingId { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventEnd { get; set; }

        public DateTime BookingStart { get; set; }

        public DateTime BookingEnd { get; set; }

        public string BookedFor { get; set; } = string.Empty;

        public string SjaEventDipsId { get; set; } = string.Empty;

        public string SjaEventName { get; set; } = string.Empty;

        public string SjaEventType { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public List<CheckBoxListItem> Options { get; set; } = new();

        internal static DataModel FromDomain(Booking booking) => new()
        {
            BookingId = booking.Id?.Value ?? throw new NullReferenceException(),

            BookingStart = booking.BookingStart,
            EventStart = booking.EventStart,
            EventEnd = booking.EventEnd,
            BookingEnd = booking.BookingEnd,
            BookedFor = booking.BookedFor ?? string.Empty,
            Notes = booking.Notes,

            SjaEventName = booking.SjaEventName,
            SjaEventDipsId = booking.SjaEventDipsId,
            SjaEventType = booking.SjaEventType
        };

        internal static void UpdateBooking(Booking booking, DataModel data)
        {
            booking.BookingStart = data.BookingStart;
            booking.EventStart = data.EventStart;
            booking.EventEnd = data.EventEnd;
            booking.BookingEnd = data.BookingEnd;
            booking.BookedFor = data.BookedFor;
            booking.Notes = data.Notes;

            booking.SjaEventName = data.SjaEventName;
            booking.SjaEventDipsId = data.SjaEventDipsId;
            booking.SjaEventType = data.SjaEventType;
        }
    }



}
