using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Items;

public class DeleteModel : PageModel
{
    private readonly IItemService _itemService;

    public Item Item { get; set; } = default!;

    public DeleteModel(IItemService itemService)
    {
        _itemService = itemService;
    }


    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var itemId = new Item.ItemId(id.Value);

        var item = await _itemService.GetById(itemId);
        if (item == null)
        {
            return NotFound();
        }

        Item = item;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var itemId = new Item.ItemId(id.Value);
        await _itemService.Delete(itemId);

        return RedirectToPage("./Index");
    }
}
