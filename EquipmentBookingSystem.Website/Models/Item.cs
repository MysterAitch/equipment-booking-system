using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentBookingSystem.Website.Models;

public class Item : BaseEntity
{
    public Guid Id { get; init; }

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public HashSet<Booking> Bookings { get; set; } = new();

    public HashSet<ItemIdentifier> Identifiers { get; set; } = new();

    [NotMapped]
    public IEnumerable<ItemIdentifier> CurrentIdentifiers => Identifiers
        .Where(x => (x.From ?? DateTime.MinValue) <= DateTime.Today)
        .Where(x => (x.To ?? DateTime.MaxValue) > DateTime.Today);

    [NotMapped]
    public IEnumerable<ItemIdentifier> CurrentSerialNumbers => CurrentIdentifiers.Where(i => i.Type == "Serial Number");

    [NotMapped]
    public IEnumerable<ItemIdentifier> CurrentProCloudAssetIds => CurrentIdentifiers
            .Where(i => i.Type == "ProCloud Asset ID");

    [NotMapped]
    public IEnumerable<ItemIdentifier> CurrentCallSigns => CurrentIdentifiers.Where(i => i.Type == "Call Sign");

    [NotMapped]
    public IEnumerable<ItemIdentifier> CurrentIssis => CurrentIdentifiers.Where(i => i.Type == "ISSI");

    [Display(Name = "Damage Notes")]
    [DataType(DataType.MultilineText)]
    public String? DamageNotes { get; set; } = string.Empty;

    [Display(Name = "Notes")]
    [DataType(DataType.MultilineText)]
    public String? Notes { get; set; } = string.Empty;


    public ItemIdentifier? SerialNumber => Identifiers.FirstOrDefault(i => i.Type == "Serial Number") ?? null;

    public ItemIdentifier? ProCloudAssetId => Identifiers.FirstOrDefault(i => i.Type == "ProCloud Asset ID") ?? null;

    public ItemIdentifier? CallSign => Identifiers.FirstOrDefault(i => i.Type == "Call Sign") ?? null;

    public ItemIdentifier? Issi => Identifiers.FirstOrDefault(i => i.Type == "ISSI") ?? null;


    public virtual string DisplayName()
    {
        string val = "";
        if (CallSign is not null)
        {
            val += CallSign.Value;
        }

        if (Issi is not null)
        {
            if (!string.IsNullOrWhiteSpace(val))
            {
                val += " / ";
            }

            val += Issi.Value;
        }

        // concatenate all remaining identifiers, and wrap in brackets ()
        var otherIdentifiers = string.Join(", ", Identifiers
            .Where(i => i != CallSign && i != Issi)
            .Select(i => i.Type + ": " + i.Value)
        );

        if (!string.IsNullOrWhiteSpace(otherIdentifiers))
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                val += otherIdentifiers;
            }
            else
            {
                val += " (";
                val += otherIdentifiers;
                val += ")";
            }
        }

        val = $"{Manufacturer} {Model}: {val}";

        return val;
    }
}
