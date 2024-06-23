using System;
using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Persistence.Models;

public class BaseEntity
{
    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Display(Name = "Updated Date")]
    public DateTime UpdatedDate { get; set; } = DateTime.Now;

    [Display(Name = "Created By")]
    public string CreatedBy { get; set; } = string.Empty;

    [Display(Name = "Updated By")]
    public string UpdatedBy { get; set; } = string.Empty;
}
