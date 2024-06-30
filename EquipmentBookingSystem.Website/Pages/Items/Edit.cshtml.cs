using System.ComponentModel.DataAnnotations;
using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;
// using EquipmentBookingSystem.Website.Pages.Shared;
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

    [BindProperty]
    public DataModel Data { get; set; } = new();


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

        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();
        Data = DataModel.FromDomain(item);

        var identifiers = await _itemService
            .GetIdentifiersForItem(currentUser, itemId);

        Data.OrderedIdentifiers = identifiers
            .OrderBy(i => i.Type)
            .ThenBy(i => i.From)
            .ThenBy(i => i.To)
            .ThenBy(i => i.Value)
            .Select(i => DataModel.Identifier.FromDomain(i))
            .ToList();


        if (Data.OrderedIdentifiers.Count == 0)
        {
            Data.OrderedIdentifiers.Add(new DataModel.Identifier()
            {
                Id = Guid.Empty,
                Type = "Serial Number",
                Value = string.Empty,
                From = DateTime.Today,
                To = null,
            });

            Data.OrderedIdentifiers.Add(new DataModel.Identifier()
            {
                Id = Guid.Empty,
                Type = "ProCloud Asset ID",
                Value = string.Empty,
                From = DateTime.Today,
                To = null,
            });

            Data.OrderedIdentifiers.Add(new DataModel.Identifier()
            {
                Id = Guid.Empty,
                Type = "Call Sign",
                Value = string.Empty,
                From = DateTime.Today,
                To = null,
            });

            Data.OrderedIdentifiers.Add(new DataModel.Identifier()
            {
                Id = Guid.Empty,
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


        if (!ModelState.IsValid)
        {
            // Redisplay the form with the submitted data
            return Page();
        }

        var currentUser = _userService.GetCurrentUser() ?? throw new UnidentifiedUserException();
        DataModel.UpdateItem(item, Data);


        await _itemService.Update(itemId, currentUser, item);
        // await _itemService.UpdateIdentifiersForItem(currentUser, itemId, selectedIdentifiersIds);

        return RedirectToPage("./Details", new { id = item.Id });
    }



    public class DataModel
    {
        public Guid ItemId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Manufacturer { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Model { get; set; } = string.Empty;

        public HashSet<Booking> Bookings { get; set; } = new();

        // public HashSet<ItemIdentifier> Identifiers { get; set; } = new();

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DamageNotes { get; set; } = string.Empty;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Notes { get; set; } = string.Empty;


        public List<Identifier> OrderedIdentifiers { get; set; } = new();


        public class Identifier
        {
            public Guid? Id { get; set; }
            public string Type { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
            public DateTime? From { get; set; }
            public DateTime? To { get; set; }

            internal static DataModel.Identifier FromDomain(ItemIdentifier identifier) => new()
            {
                Id = identifier.Id?.Value ?? throw new NullReferenceException(),
                Type = identifier.Type,
                Value = identifier.Value,
                From = identifier.From,
                To = identifier.To,
            };
        }



        internal static DataModel FromDomain(Item item) => new()
        {
            ItemId = item.Id?.Value ?? throw new NullReferenceException(),

            Manufacturer = item.Manufacturer,
            Model = item.Model,
            DamageNotes = item.DamageNotes,
            Notes = item.Notes,
        };

        internal static void UpdateItem(Item item, DataModel data)
        {
            item.Manufacturer = data.Manufacturer;
            item.Model = data.Model;
            item.DamageNotes = data.DamageNotes;
            item.Notes = data.Notes;

            UpdateIdentifiers(item, data);
        }

        internal static void UpdateIdentifiers(Item item, DataModel data)
        {
            // var submittedIdentifiers = data.Identifiers.ToList();

            // Update oldIdentifiers -- update where Id matches, remove where not in OrderedIdentifiers, and add where not in oldIdentifiers (inserting Id)
            var oldIdentifiers = item.Identifiers.ToList();
            foreach (var oldIdentifier in oldIdentifiers)
            {
                var orderedIdentifier = data.OrderedIdentifiers.SingleOrDefault(i => i.Id is not null && i.Id == oldIdentifier.Id?.Value);
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

            foreach (var orderedIdentifier in data.OrderedIdentifiers)
            {
                if (oldIdentifiers.All(i => i.Id is not null && i.Id?.Value != orderedIdentifier.Id))
                {
                    // This is a new identifier, not previously seen -- therefore add it
                    var newId = orderedIdentifier.Id is null
                        ? null as ItemIdentifierId?
                        : new ItemIdentifierId(orderedIdentifier.Id.Value);

                    var itemIdentifier = new ItemIdentifier()
                    {
                        Id = newId,
                        Type = orderedIdentifier.Type,
                        Value = orderedIdentifier.Value,
                        From = orderedIdentifier.From,
                        To = orderedIdentifier.To,
                    };
                    item.Identifiers.Add(itemIdentifier);
                }
            }
        }
    }
}
