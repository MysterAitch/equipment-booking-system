using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Website.Models;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime Modified { get; set; }
}
