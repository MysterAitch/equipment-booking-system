using EquipmentBookingSystem.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Website.Pages.Items;

public class EditModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    public EditModel(EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Item Item { get; set; } = default!;

    [BindProperty]
    public List<ItemIdentifier> OrderedIdentifiers { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        Item = item;

        OrderedIdentifiers = Item.Identifiers
            .OrderBy(i => i.From)
            .ThenBy(i => i.To)
            .ThenBy(i => i.Type)
            .ThenBy(i => i.Value)
            .ToList();

        if (OrderedIdentifiers.Count == 0)
        {
            OrderedIdentifiers.Add(new ItemIdentifier()
            {
                Id = Guid.NewGuid(),
                Type = "Serial Number",
                Value = string.Empty,
                From = null,
                To = null,
            });

            OrderedIdentifiers.Add(new ItemIdentifier()
            {
                Id = Guid.NewGuid(),
                Type = "ProCloud Asset ID",
                Value = string.Empty,
                From = null,
                To = null,
            });

            OrderedIdentifiers.Add(new ItemIdentifier()
            {
                Id = Guid.NewGuid(),
                Type = "Call Sign",
                Value = string.Empty,
                From = null,
                To = null,
            });

            OrderedIdentifiers.Add(new ItemIdentifier()
            {
                Id = Guid.NewGuid(),
                Type = "ISSI",
                Value = string.Empty,
                From = null,
                To = null,
            });
        }

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            OrderedIdentifiers = Item.Identifiers
                .OrderBy(i => i.From)
                .ThenBy(i => i.To)
                .ThenBy(i => i.Type)
                .ThenBy(i => i.Value)
                .ToList();

            return Page();
        }

        var oldItem = await _context.Item
            .Include(i => i.Bookings)
            .Include(i => i.Identifiers)
            .SingleOrDefaultAsync(m => m.Id == Item.Id);
        if (oldItem == null)
        {
            return NotFound();
        }

        oldItem.Name = Item.Name;
        oldItem.Manufacturer = Item.Manufacturer;
        oldItem.Model = Item.Model;

        // Update oldIdentifiers -- update where Id matches, remove where not in OrderedIdentifiers, and add where not in oldIdentifiers (inserting Id)
        var oldIdentifiers = oldItem.Identifiers.ToList();
        foreach (var oldIdentifier in oldIdentifiers)
        {
            var orderedIdentifier = OrderedIdentifiers.SingleOrDefault(i => i.Id == oldIdentifier.Id);
            if (orderedIdentifier == null)
            {
                oldItem.Identifiers.Remove(oldIdentifier);
            }
            else
            {
                oldIdentifier.Type = orderedIdentifier.Type;
                oldIdentifier.Value = orderedIdentifier.Value;
                oldIdentifier.From = orderedIdentifier.From;
                oldIdentifier.To = orderedIdentifier.To;
            }
        }

        foreach (var orderedIdentifier in OrderedIdentifiers)
        {
            if (oldIdentifiers.All(i => i.Id != orderedIdentifier.Id))
            {
                var itemIdentifier = new ItemIdentifier()
                {
                    Id = Guid.Empty == orderedIdentifier.Id ? Guid.NewGuid() : orderedIdentifier.Id,
                    Type = orderedIdentifier.Type,
                    Value = orderedIdentifier.Value,
                    From = orderedIdentifier.From,
                    To = orderedIdentifier.To,
                };
                oldItem.Identifiers.Add(itemIdentifier);

                _context.ItemIdentifiers.Add(itemIdentifier);
            }
        }

        oldItem.UpdatedDate = DateTime.Now;

        var x = User.Identity?.Name ?? throw new UnidentifiedUserException();
        oldItem.UpdatedBy = x;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ItemExists(Item.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Details", new { id = Item.Id });
    }

    private bool ItemExists(Guid id)
    {
        return _context.Item.Any(e => e.Id == id);
    }
}
