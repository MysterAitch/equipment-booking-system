using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Website.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Items;

public class IndexModel : PageModel
{
    private readonly IItemService _itemService;
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;

    public IndexModel(IItemService itemService, IBookingService bookingService, IUserService userService)
    {
        _itemService = itemService;
        _bookingService = bookingService;
        _userService = userService;
    }

    public IList<Item> Items { get; set; } = new List<Item>();
    public IList<Booking> BookingsForItems { get; set; } = new List<Booking>();

    public async Task OnGetAsync()
    {
        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();
        Items = await _itemService.GetVisibleItems(currentUser);

        var itemIds = Items.Select(i => i.Id)
            .Where(id => id is not null)
            .ToList();
        var bookings = await _bookingService.BookingsForItems(currentUser, itemIds);
        BookingsForItems = bookings.ToList();
    }
}
