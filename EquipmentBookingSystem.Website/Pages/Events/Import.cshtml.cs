using System.ComponentModel.DataAnnotations;
using EquipmentBookingSystem.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EquipmentBookingSystem.Website.Pages.Events;

public class ImportModel : PageModel
{
    private readonly EquipmentBookingSystem.Website.Data.WebsiteDbContext _context;

    private readonly DipsJsonToEvents _dipsJsonToEvents;

    // property to store JSON string submitted via form (multiline text box)
    [BindProperty]
    [Required]
    [DataType(DataType.MultilineText)]
    public string Json { get; set; } = default!;

    public ImportModel(DipsJsonToEvents dipsJsonToEvents, EquipmentBookingSystem.Website.Data.WebsiteDbContext context)
    {
        _context = context;
        _dipsJsonToEvents = dipsJsonToEvents;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var importedEvents = _dipsJsonToEvents.ConvertJsonToEvents(Json)
            .ToList();

        var modifiedCount = 0;
        var newCount = 0;


        var importedDipsIds = importedEvents.Select(e => e.DipsId).ToList();
        var importedEventGuids = importedEvents.Select(e2 => e2.Id).ToList();

        var existingMatchingEvents = await _context.Events
            .Where(e => importedDipsIds.Contains(e.DipsId) || importedEventGuids.Contains(e.Id))
            .ToListAsync();

        // Using list of submitted DIPS event IDs, update where it already exists, and add / create new if not
        foreach (var importedEvent in importedEvents)
        {
            var existingEvent = existingMatchingEvents.SingleOrDefault(e2 => e2.Id == importedEvent.Id || e2.DipsId == importedEvent.DipsId);
            if (existingEvent != null)
            {
                modifiedCount++;
                if (importedEvent.Id == Guid.Empty)
                {
                    importedEvent.Id = existingEvent.Id;
                }
                _context.Entry(existingEvent).CurrentValues.SetValues(importedEvent);
            }
            else
            {
                newCount++;
                _context.Events.Add(importedEvent);
            }
        }

        // _context.Events.Add(Event);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
