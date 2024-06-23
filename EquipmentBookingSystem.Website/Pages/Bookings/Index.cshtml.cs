using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Website.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Bookings;

public class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;

    public IndexModel(IBookingService bookingService, IUserService userService)
    {
        _bookingService = bookingService;
        _userService = userService;
    }

    public IList<Booking> Bookings { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();
        Bookings = (await _bookingService.GetVisibleBookings(currentUser)).ToList();
    }
}
