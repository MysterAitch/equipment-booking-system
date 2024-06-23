using System.Security.Claims;
using EquipmentBookingSystem.Application.Services;
using EquipmentBookingSystem.Domain.Models;

namespace EquipmentBookingSystem.Website.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentUserId()
    {
        var currentUser = GetCurrentUser();
        return currentUser?.Id;
    }

    public string? GetCurrentUserName()
    {
        var currentUser = GetCurrentUser();
        return currentUser?.Name;
    }

    public string? GetCurrentUserEmail()
    {
        var currentUser = GetCurrentUser();
        return currentUser?.Email;
    }

    public User? GetCurrentUser()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var httpContextUser = httpContext?.User;

        var currentUserId = httpContextUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUserName = httpContextUser?.Identity?.Name;
        var currentUserEmail = httpContextUser?.FindFirst(ClaimTypes.Email)?.Value;

        return new User
        {
            Id = currentUserId,
            Name = currentUserName,
            Email = currentUserEmail
        };
    }
}
