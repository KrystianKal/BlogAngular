using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

[Authorize]
[Route("/articles")]
public class DeleteArticleController(IMediator mediator) : ControllerBase
{
    [HttpDelete("{slug}")]
    public async Task Delete(string slug, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new DeleteArticleCommand(slug), cancellationToken));
}

public record DeleteArticleCommand(string Slug) : IRequest<Unit>;

public class DeleteArticleCommandHandler(BlogDbContext context, IUserAccessor userAccessor)
    : IRequestHandler<DeleteArticleCommand, Unit>
{
    public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var articleToDelete = await context.Articles.SingleOrDefaultAsync(x => x.Slug.Equals(request.Slug));
        if (articleToDelete is null)
        {
            throw new ArticleNotFoundException(request.Slug);
        }

        var currentUserIsNotTheAuthor = !articleToDelete.AuthorId.Value.ToString().Equals(userAccessor.GetCurrentUserId());
        if (currentUserIsNotTheAuthor)
        {
            throw new CurrentUserIsNotTheAuthorException();
        }

        context.Articles.Remove(articleToDelete);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

}
