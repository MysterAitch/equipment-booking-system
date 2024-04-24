using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Website.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime Modified { get; set; }
}
