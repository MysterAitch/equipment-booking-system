using EquipmentBookingSystem.Domain.Models;

namespace EquipmentBookingSystem.Application.Services;

public interface IUserService
{
    string? GetCurrentUserId();
    string? GetCurrentUserName();
    string? GetCurrentUserEmail();
    User? GetCurrentUser();
}
