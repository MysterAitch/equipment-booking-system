using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
using EquipmentBookingSystem.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EquipmentBookingSystem.Website.Pages.Items;

public class EditModel : PageModel
{
    private readonly IItemService _itemService;

    private IUserService _userService;

    public EditModel(IItemService itemService, IUserService userService)
    {
        _itemService = itemService;
        _userService = userService;
    }

    public Guid ItemId { get; set; }

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public HashSet<Booking> Bookings { get; set; } = new();

    public HashSet<ItemIdentifier> Identifiers { get; set; } = new();

    public String? DamageNotes { get; set; } = string.Empty;

    public String? Notes { get; set; } = string.Empty;


    [BindProperty] public List<ItemIdentifier> OrderedIdentifiers { get; set; } = new();

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

        ItemId = itemId.Value;
        Manufacturer = item.Manufacturer;
        Model = item.Model;
        DamageNotes = item.DamageNotes;
        Notes = item.Notes;

        // TODO: Expand with view model for identifiers
        Identifiers = item.Identifiers;


        OrderedIdentifiers = item.Identifiers
            .OrderBy(i => i.Type)
            .ThenBy(i => i.From)
            .ThenBy(i => i.To)
            .ThenBy(i => i.Value)
            .ToList();

        if (OrderedIdentifiers.Count == 0)
        {
            OrderedIdentifiers.Add(new ItemIdentifier()
            {
                Id = new ItemIdentifierId(Guid.NewGuid()),
                Type = "Serial Number",
                Value = string.Empty,
                From = DateTime.Today,
                To = null,
            });

            OrderedIdentifiers.Add(new ItemIdentifier()
            {
                Id = new ItemIdentifierId(Guid.NewGuid()),
                Type = "ProCloud Asset ID",
                Value = string.Empty,
                From = DateTime.Today,
                To = null,
            });

            OrderedIdentifiers.Add(new ItemIdentifier()
            {
                Id = new ItemIdentifierId(Guid.NewGuid()),
                Type = "Call Sign",
                Value = string.Empty,
                From = DateTime.Today,
                To = null,
            });

            OrderedIdentifiers.Add(new ItemIdentifier()
            {
                Id = new ItemIdentifierId(Guid.NewGuid()),
                Type = "ISSI",
                Value = string.Empty,
                From = DateTime.Today,
                To = null,
            });
        }

        return Page();
    }


    public async Task<IActionResult> OnPostAsync(Guid? id /*, List<Identifier> identifiers*/)
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

        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();

        if (!ModelState.IsValid)
        {
            OrderedIdentifiers = item.Identifiers
                .OrderBy(i => i.From)
                .ThenBy(i => i.To)
                .ThenBy(i => i.Type)
                .ThenBy(i => i.Value)
                .ToList();

            return Page();
        }


        item.Manufacturer = Manufacturer;
        item.Model = Model;
        item.DamageNotes = DamageNotes;
        item.Notes = Notes;


        // Update oldIdentifiers -- update where Id matches, remove where not in OrderedIdentifiers, and add where not in oldIdentifiers (inserting Id)
        var oldIdentifiers = item.Identifiers.ToList();
        foreach (var oldIdentifier in oldIdentifiers)
        {
            var orderedIdentifier = OrderedIdentifiers.SingleOrDefault(i => i.Id == oldIdentifier.Id);
            if (orderedIdentifier == null)
            {
                item.Identifiers.Remove(oldIdentifier);
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
                // This is a new identifier, not previously seen -- therefore add it
                var itemIdentifier = new ItemIdentifier()
                {
                    Id = Guid.Empty == orderedIdentifier.Id?.Value
                        ? new ItemIdentifierId(Guid.NewGuid())
                        : orderedIdentifier.Id,
                    Type = orderedIdentifier.Type,
                    Value = orderedIdentifier.Value,
                    From = orderedIdentifier.From,
                    To = orderedIdentifier.To,
                };
                item.Identifiers.Add(itemIdentifier);

                var newIdentifier = await _itemService.CreateIdentifier(itemIdentifier);
            }
        }

        await _itemService.Update(itemId, currentUser, item);

        return RedirectToPage("./Details", new { id = item.Id?.Value ?? throw new Exception("Item ID not found") });
    }
}
