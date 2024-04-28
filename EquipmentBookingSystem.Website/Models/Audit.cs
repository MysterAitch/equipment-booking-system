namespace EquipmentBookingSystem.Website.Models;

public class Audit
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public DateTime ChangeTimeUtc { get; set; } = DateTime.UtcNow;
    public Guid? EntityId { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
