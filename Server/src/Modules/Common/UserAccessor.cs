using System.Security.Claims;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Users;
using BlogBackend.Modules.Users.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Common;

public sealed class UserAccessor(IHttpContextAccessor httpContextAccessor, BlogDbContext ctx) : IUserAccessor
{
    public string? GetCurrentUserId()
        => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public async Task<User> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId is null) throw new UnauthorizedAccessException("Tried to get current user for an unauthenticated user");
        
        var currentUser = await ctx.Users
            .SingleOrDefaultAsync(x => x.UserId == UserId.Parse(currentUserId), cancellationToken: cancellationToken);
        return
            currentUser ??
            throw new CurrentUserNotFoundException(UserId.Parse(currentUserId));
    }

    public bool IsAuthenticated()
        => GetCurrentUserId() != null ;
}
