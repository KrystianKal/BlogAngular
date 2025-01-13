using BlogBackend.Modules.Users;

namespace BlogBackend.Modules.Common;

public interface IUserAccessor
{
    string? GetCurrentUserId();
    Task<User> GetCurrentUser(CancellationToken cancellationToken);
    bool IsAuthenticated();
}
