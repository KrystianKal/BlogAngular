using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogBackend.Modules.Articles.Utils;

namespace BlogBackend.Modules.Articles.Features;

[Authorize]
[Route("articles")]
public class UnfavoriteArticleController(IMediator mediator) : ControllerBase
{
    [HttpDelete("{slug}/favorite")]
    public async Task<ArticleResponse> Unfavorite(string slug, CancellationToken cancellationToken)
        => await mediator.Send(new UnfavoriteArticleCommand(slug), cancellationToken);
}


public record UnfavoriteArticleCommand(string Slug)
    : IRequest<ArticleResponse>;

public class UnfavoriteArticleCommandHandler(BlogDbContext context, IUserAccessor userAccessor, IAuthorService authorService)
    : IRequestHandler<UnfavoriteArticleCommand, ArticleResponse>
{
    public async Task<ArticleResponse> Handle(UnfavoriteArticleCommand request, CancellationToken cancellationToken)
    {
        var article = await context.Articles
            .Include(x => x.ArticleFavoriteds)
            .Include(x=> x.Tags)
            .SingleOrDefaultAsync(x => x.Slug.Equals(request.Slug), cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException(request.Slug);
        }

        var currentUserId = userAccessor.GetCurrentUserId()!;

        var isFavorited = article.ArticleFavoriteds.Any(x => x.UserId == UserId.Parse(currentUserId));
        if (!isFavorited)
        {
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, new { User = "Cannot unfavorite article that is not favorited" });
        }

        article.Unfavorite(UserId.Parse(currentUserId));
        context.Articles.Update(article);
        await context.SaveChangesAsync(cancellationToken);

        var author = await authorService.GetAuthor(article.AuthorId, cancellationToken);
        return new ArticleResponse(article, author,false);
    }
}
