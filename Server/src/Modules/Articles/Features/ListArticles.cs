using BlogBackend.Modules.Articles.Domain;
using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Common.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

[Route("Articles")]
public class ListArticlesController(IMediator mediator)
    : ControllerBase
{
    [HttpGet]
    public async Task<ArticlesResponse> List(
        [FromQuery] string? tag,
        [FromQuery] string? author,
        [FromQuery] string? favorited,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0,
        CancellationToken token = default)
        => await mediator.Send(new ListArticlesQuery(tag, author, favorited, limit, offset), token);
}

public record ListArticlesQuery(string? Tag, string? Author, string? Favorited, int limit, int offset)
    : IRequest<ArticlesResponse>;

public class ListArticlesQueryHandler(BlogDbContext context, IUserAccessor userAccessor, IAuthorService authorService)
    : IRequestHandler<ListArticlesQuery, ArticlesResponse>
{
    public async Task<ArticlesResponse> Handle(ListArticlesQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = userAccessor.GetCurrentUserId();

        var query = context.Articles
            .Include(x => x.Tags)
            .Include(x => x.ArticleFavoriteds)
            .AsQueryable();

        //filter by tag
        if (!string.IsNullOrEmpty(request.Tag))
        {
            query = query.Where(x => x.Tags.Any(t => t.Name.ToLower() == request.Tag.ToLower()));
        }
        //filter author
        if (!string.IsNullOrEmpty(request.Author))
        {
            var authorId = await UserHelper.GetUserIdFromUsername(request.Author, context, cancellationToken);

            query = query.Where(x => x.AuthorId == authorId);
        }

        //filter by favorited
        if (!string.IsNullOrEmpty(request.Favorited))
        {
            var favoritedByUserId = await UserHelper.GetUserIdFromUsername(request.Favorited, context, cancellationToken);
            query = query.Where(a => a.ArticleFavoriteds.Any(af => af.UserId == favoritedByUserId));
        }

        //count total before paginating
        var totalArticles = await query.CountAsync(cancellationToken);

        //apply pagination
        var articles = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip(request.offset)
            .Take(request.limit)
            .ToListAsync(cancellationToken);

        var articleResponses = new List<ArticleResponse>();

        foreach (var article in articles)
        {
            var author = await authorService.GetAuthor(article.AuthorId, cancellationToken);
            bool? isFavorited = currentUserId is null
                ? null
                : article.ArticleFavoriteds.Any(x => x.UserId == UserId.Parse(currentUserId));
            articleResponses.Add(new ArticleResponse(article, author, isFavorited));
        }

        return new ArticlesResponse(articleResponses.ToArray(), totalArticles );
    }
}
