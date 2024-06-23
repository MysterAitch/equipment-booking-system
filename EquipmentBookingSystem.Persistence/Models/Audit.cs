using System;
using System.ComponentModel.DataAnnotations.Schema;
using EquipmentBookingSystem.Domain.Models;

namespace EquipmentBookingSystem.Persistence.Models;

public class Audit
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public DateTime ChangeTimeUtc { get; set; } = DateTime.UtcNow;
    public Guid? EntityId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    // to domain

    public RecordChangeEntry ToDomain()
    {
        var domainRecordChangeEntry = new RecordChangeEntry()
        {
            Id = Id,
            UserEmail = UserEmail,
            ChangeTimeUtc = ChangeTimeUtc,
            EntityId = EntityId,
            TableName = TableName,
            PropertyName = PropertyName,
            PropertyType = PropertyType,
            OldValue = OldValue,
            NewValue = NewValue,
        };

        return domainRecordChangeEntry;
    }
}
