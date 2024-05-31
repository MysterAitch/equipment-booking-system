using System.ComponentModel.DataAnnotations;

namespace EquipmentBookingSystem.Website.Models;

public class Booking : BaseEntity
{
    public Guid Id { get; init; }

    public HashSet<Item> Items { get; } = new();

    [Display(Name = "Booking Start")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:ddd dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime BookingStart { get; set; }

    [Display(Name = "Event Start")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:ddd dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime EventStart { get; set; }

    [Display(Name = "Event End")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:ddd dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime EventEnd { get; set; }

    [Display(Name = "Booking End")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:ddd dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime BookingEnd { get; set; }

    [Display(Name = "Booked By")]
    [DataType(DataType.EmailAddress)]
    public string BookedBy { get; set; } = string.Empty;

    [Display(Name = "Booked For")]
    [DataType(DataType.EmailAddress)]
    public string BookedFor { get; set; } = string.Empty;

    [Display(Name = "SJA Event DIPS ID")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Required(AllowEmptyStrings = true)]
    public string SjaEventDipsId { get; set; } = string.Empty;

    [Display(Name = "SJA Event Name")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Required(AllowEmptyStrings = true)]
    public string SjaEventName { get; set; } = string.Empty;

    [Display(Name = "SJA Event Type")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Required(AllowEmptyStrings = true)]
    public string SjaEventType { get; set; } = string.Empty;


    [Display(Name = "Notes")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [Required(AllowEmptyStrings = true)]
    [DataType(DataType.MultilineText)]
    public string Notes { get; set; } = string.Empty;

    public virtual string DisplayName()
    {
        // return $"{BookingStart} - {BookingEnd} ({Id})";
        return $"{SjaEventName} ({BookingStart} - {BookingEnd})";
    }
}
