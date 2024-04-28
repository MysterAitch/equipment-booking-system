namespace EquipmentBookingSystem.Website.Pages_Bookings;

public class CheckBoxListItem
{
    public Guid Id { get; init; }
    public string Display { get; set; } = string.Empty;
    public bool IsChecked { get; set; } = false;
}
