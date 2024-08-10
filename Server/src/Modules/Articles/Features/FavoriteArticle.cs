using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

[Authorize]
[Route("articles")]
public class FavoriteArticleController(IMediator mediator)
    : ControllerBase
{
    [HttpPost("{slug}/favorite")]
    public async Task<ArticleResponse> Favorite(string slug, CancellationToken cancellationToken)
        => await mediator.Send(new FavoriteArticleCommand(slug), cancellationToken);
}

public record FavoriteArticleCommand(string Slug) : IRequest<ArticleResponse>;

public class FavoriteArticleCommandHandler(BlogDbContext context, IUserAccessor userAccessor, IAuthorService authorService)
    : IRequestHandler<FavoriteArticleCommand, ArticleResponse>
{
    public async Task<ArticleResponse> Handle(FavoriteArticleCommand request, CancellationToken cancellationToken)
    {
        var articleToFavorite = await context.Articles
            .Include(x => x.ArticleFavoriteds)
            .Include(x => x.Tags)
            .SingleOrDefaultAsync(x => x.Slug.Equals(request.Slug));

        if (articleToFavorite is null)
        {
            throw new ArticleNotFoundException(request.Slug);
        }
        var currentUserId = userAccessor.GetCurrentUserId()!;


        articleToFavorite.Favorite(UserId.Parse(currentUserId));

        context.Articles.Update(articleToFavorite);
        await context.SaveChangesAsync(cancellationToken);

        var author = await authorService.GetAuthor(articleToFavorite.AuthorId, cancellationToken);
        return new ArticleResponse(articleToFavorite, author,true);
    }
}
