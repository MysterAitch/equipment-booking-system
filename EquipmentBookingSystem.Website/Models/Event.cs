using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Website.Models;

public class Event
{
    public Guid Id { get; set; }

    [Display(Name = "Event Cover Start")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:ddd dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTimeOffset? EventCoverStart { get; set; }

    [Display(Name = "Event Cover End")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:ddd dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTimeOffset? EventCoverEnd { get; set; }

    [Display(Name = "Event End")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:ddd dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime? EventStart { get; set; }

    [Display(Name = "Event End")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:ddd dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime? EventEnd { get; set; }

    [Display(Name = "DIPS Event Location")]
    public string? DipsEventLocationString { get; set; } = string.Empty;
    
    [Display(Name = "DIPS Event Postcode")]
    public string? DipsEventPostcode { get; set; } = string.Empty;

    [Display(Name = "DIPS Event Type")]
    public string? DipsEventType { get; set; }
    
    [Display(Name = "DIPS Is Event Deleted")]
    public bool DipsIsDeleted { get; set; } = false;

    [Display(Name = "DIPS Event ID")]
    public string? DipsId { get; set; } = string.Empty;
    
    [Display(Name = "DIPS Event Title")]
    public string? DipsEventTitle { get; set; }

    [Display(Name = "DIPS Allocated Area")]
    public string? DipsAllocatedArea { get; set; }
    
    [Display(Name = "DIPS Allocated District")]
    public string? DipsAllocatedDistrict { get; set; }
}
