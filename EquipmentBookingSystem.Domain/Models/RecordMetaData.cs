using System;

namespace EquipmentBookingSystem.Domain.Models;

public class RecordMetaData
{

    public DateTime CreatedAt { get; init; }
    public string? CreatedBy { get; set; }

    public DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; set; }
}
