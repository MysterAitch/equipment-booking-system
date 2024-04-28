using System.Security.Claims;

namespace EquipmentBookingSystem.Website.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentUserId() => _httpContextAccessor.HttpContext?
        .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? GetCurrentUserName() => _httpContextAccessor.HttpContext?
        .User.Identity?.Name;

    public string? GetCurrentUserEmail()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var httpContextUser = httpContext?.User;
        var findFirstEmailClaim = httpContextUser?.FindFirst(ClaimTypes.Email);
        var currentUserEmail = findFirstEmailClaim?.Value;

        return currentUserEmail;
    }
}
