using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Items;

public class DetailsModel : PageModel
{
    private readonly IItemService _itemService;

    private readonly IBookingService _bookingService;

    private readonly IUserService _userService;

    public Item Item { get; set; } = default!;

    public List<Booking> BookingsForItem { get; set; } = new();

    public List<RecordChangeEntry> Changes { get; set; } = new();

    public DetailsModel(IItemService itemService, IBookingService bookingService, IUserService userService)
    {
        _itemService = itemService;
        _bookingService = bookingService;
        _userService = userService;
    }


    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null || id.Value == Guid.Empty)
        {
            return BadRequest();
        }


        var itemId = new ItemId(id.Value);

        var item = await _itemService.GetById(itemId);
        if (item == null)
        {
            return NotFound();
        }

        Item = item;

        var currentUser = _userService.GetCurrentUser();
        BookingsForItem = (await _bookingService.BookingsForItem(currentUser, itemId)).ToList();

        var changes = await _itemService.ChangesForItem(itemId);
        Changes = changes.ToList();

        return Page();
    }
}
