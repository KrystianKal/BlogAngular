using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Profiles.Utils;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

[Route("articles")]
public class GetArticleController(IMediator mediator) : ControllerBase
{
    [HttpGet("{Slug}")]
    public async Task<ArticleResponse> Get(string Slug, CancellationToken cancellationToken)
        => await mediator.Send(new GetArticleQuery(Slug.ToLower()), cancellationToken);
}

public record GetArticleQuery(string Slug) : IRequest<ArticleResponse>;

public class GetArticleQueryHandler(BlogDbContext context, IUserAccessor userAccessor, IAuthorService authorService)
    : IRequestHandler<GetArticleQuery, ArticleResponse>
{
    public async Task<ArticleResponse> Handle(GetArticleQuery request, CancellationToken cancellationToken)
    {
        var article = await context.Articles
            .Include(x => x.ArticleFavoriteds)
            .Include(x => x.Tags)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Slug.Equals(request.Slug), cancellationToken);
        if (article is null)
        {
            throw new ArticleNotFoundException(request.Slug);
        }

        var currentUserId = userAccessor.GetCurrentUserId();

        var author = await authorService.GetAuthor(article.AuthorId, cancellationToken);

        if (currentUserId == null)
        {
            return new ArticleResponse(article, author);
        }

        var isFavorited = article.ArticleFavoriteds.Any(x => x.UserId == UserId.Parse(currentUserId));

        return new ArticleResponse(article, author, isFavorited);
    }
}
