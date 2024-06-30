using System;
using System.Collections.Generic;
using System.Linq;
using StronglyTypedIds;

namespace EquipmentBookingSystem.Domain.Models;

public class Item
{
    public ItemId? Id { get; init; }

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public HashSet<ItemIdentifier> Identifiers { get; set; } = new();

    public IEnumerable<ItemIdentifier> CurrentIdentifiers => Identifiers
        .Where(x => (x.From ?? DateTime.MinValue) <= DateTime.Today)
        .Where(x => (x.To ?? DateTime.MaxValue) > DateTime.Today);

    public IEnumerable<ItemIdentifier> CurrentSerialNumbers => CurrentIdentifiers.Where(i => i.Type == "Serial Number");

    public IEnumerable<ItemIdentifier> CurrentProCloudAssetIds => CurrentIdentifiers
        .Where(i => i.Type == "ProCloud Asset ID");

    public IEnumerable<ItemIdentifier> CurrentCallSigns => CurrentIdentifiers.Where(i => i.Type == "Call Sign");

    public IEnumerable<ItemIdentifier> CurrentIssis => CurrentIdentifiers.Where(i => i.Type == "ISSI");

    public string DamageNotes { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;



    public RecordMetaData RecordMetaData { get; set; } = default!;


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


[StronglyTypedId(Template.Guid)]
public partial struct ItemId { }
