using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Utils;

public interface IAuthorService
{
    Task<Author> GetAuthor(UserId authorId, CancellationToken cancellationToken = default);
}
public class AuthorService(BlogDbContext context, IUserAccessor userAccessor): IAuthorService
{
    public async Task<Author> GetAuthor(UserId authorId, CancellationToken cancellationToken)
    {
        //is null if user is not authenticated
        var currentUserId =userAccessor.GetCurrentUserId();

        var author = await context.Profiles
            .AsNoTracking()
            .Include(x => x.Followers)
            .Include(x => x.User)
            .Where(x => x.UserId == authorId)
            .Select(x => new Author
                (
                x.ProfileName,
                x.User.Name,
                x.Bio,
                x.Image == null ? null : x.Image.Value,
                currentUserId != null && x.Followers.Any(
                    f => f.Follower.UserId == UserId.Parse(currentUserId))
                ))
            .SingleOrDefaultAsync(cancellationToken);

        if (author is null)
        {
            throw new Exception("Author profile not found");
        }
        return author;

    }
}
