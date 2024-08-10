using BlogBackend.Modules.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Common.Utils;

public static class UserHelper
{
    public static async Task<UserId> GetUserIdFromUsername(string name, BlogDbContext context, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .Where(x => x.Name.ToLower() == name.ToLower())
            .Select(x => x.UserId)
            .SingleAsync(cancellationToken);
    }
}
