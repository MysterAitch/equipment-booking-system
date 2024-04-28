namespace EquipmentBookingSystem.Website.Services;

public interface IUserService
{
    string? GetCurrentUserId();
    string? GetCurrentUserName();
    string? GetCurrentUserEmail();
}
