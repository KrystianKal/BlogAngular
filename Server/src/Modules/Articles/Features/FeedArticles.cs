using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlogBackend.Modules.Articles.Features;

[Authorize]
[Route("Articles")]
//articles by followed profiles
public class FeedArticlesController(IMediator mediator) : ControllerBase
{
    [HttpGet("feed")]
    public async Task<ArticlesResponse> Feed([FromQuery] int limit = 20, [FromQuery] int offset = 0, CancellationToken token = default)
        => await mediator.Send(new FeedArticlesQuery(limit, offset), token);
}

public record FeedArticlesQuery(int Limit, int Offset) : IRequest<ArticlesResponse>;

public class FeedArticlesQueryHandler(BlogDbContext context, IUserAccessor userAccessor, IAuthorService authorService)
    : IRequestHandler<FeedArticlesQuery, ArticlesResponse>
{
    public async Task<ArticlesResponse> Handle(FeedArticlesQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = userAccessor.GetCurrentUserId()!;
            // .Include(x => x.Following)
        var currentUserFollowList = await context.Profiles
            .AsNoTracking()
            .Where(x => x.UserId == UserId.Parse(currentUserId))
            .SelectMany(x => x.Following.Select(f => f.Following.UserId))
            .ToListAsync(cancellationToken);

        var query = context.Articles
            .AsNoTracking()
            .Where(x => currentUserFollowList.Contains(x.AuthorId))
            .OrderByDescending(a => a.CreatedAt);

        var totalArticles = await query.CountAsync(cancellationToken);

        var articles = await query.Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync (cancellationToken);
        
        var articleResponses = new List<ArticleResponse>();
        foreach(var article in articles ){
            var author = await authorService.GetAuthor(article.AuthorId, cancellationToken);
            articleResponses.Add(new ArticleResponse(article,author));
        }

        return new ArticlesResponse(articleResponses.ToArray(), totalArticles);
    }
}
