namespace EquipmentBookingSystem.Website.Pages.Shared;

public class CheckBoxListItem
{
    public Guid Id { get; init; }
    public string Display { get; set; } = string.Empty;
    public bool IsChecked { get; set; } = false;
}
