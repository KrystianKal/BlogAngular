using System.Security.Claims;

namespace BlogBackend.Modules.Common;

public class UserAccessor(IHttpContextAccessor httpContextAccessor) : IUserAccessor
{
    public string? GetCurrentUserId()
        => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
