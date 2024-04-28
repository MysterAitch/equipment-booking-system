using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EquipmentBookingSystem.Website.Models;

namespace EquipmentBookingSystem.Website.Pages_Items;

public class CreateModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    public CreateModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        Item = new Item();
        return Page();
    }

    [BindProperty]
    public Item Item { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        var currentUser = User.Identity?.Name ?? throw new UnidentifiedUserException();
        Item.CreatedBy = currentUser;
        Item.UpdatedBy = currentUser;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Item.Add(Item);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
