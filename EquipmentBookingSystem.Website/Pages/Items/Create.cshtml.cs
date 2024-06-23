using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Items;

public class CreateModel : PageModel
{
    private readonly IItemService _itemService;
    private readonly IUserService _userService;

    public CreateModel(IItemService itemService, IUserService userService)
    {
        _itemService = itemService;
        _userService = userService;
    }


    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;


    public IActionResult OnGet()
    {
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var item = new Item
        {
            Manufacturer = Manufacturer,
            Model = Model,
        };

        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();
        var newItem = await _itemService.CreateNew(currentUser, item);

        var newId = newItem.Id?.Value;
        if (newId is null || newId == Guid.Empty)
        {
            throw new InvalidOperationException("Item ID is null");
        }

        return RedirectToPage("./Edit", new {id = newId});
    }
}
