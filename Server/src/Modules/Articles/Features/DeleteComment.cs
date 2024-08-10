using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

[Route("articles")]
public class DeleteCommentController(IMediator mediator) : ControllerBase
{
    [HttpDelete("{slug}/comments/{id}")]
    public async Task<ActionResult> Delete(string slug, string id, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new DeleteCommentCommand(slug, id), cancellationToken));
}

public record DeleteCommentCommand(string Slug, string Id) : IRequest<Unit>;

public class DeleteCommentCommandHandler(BlogDbContext context, IUserAccessor userAccessor)
    : IRequestHandler<DeleteCommentCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var article = await context.Articles
            .Include(x => x.Comments)
            .SingleOrDefaultAsync(x => x.Slug.Equals(request.Slug));

        if (article is null)
            throw new ArticleNotFoundException(request.Slug);

        var commentToRemove = article.Comments
            .SingleOrDefault(x => x.CommentId.Value.ToString().Equals(request.Id));

        if (commentToRemove is null)
        {
            throw new ApiException(System.Net.HttpStatusCode.NotFound,
                new { Comment = $"Comment with id: \"{request.Id}\" not found" });
        }

        var currentUserIsNotTheAuthor = !userAccessor.GetCurrentUserId()!
            .Equals(commentToRemove.AuthorId.Value.ToString());

        if (currentUserIsNotTheAuthor)
        {
            throw new CurrentUserIsNotTheAuthorException();
        }

        article.Comments.Remove(commentToRemove);

        await context.SaveChangesAsync();

        return Unit.Value;
    }
}
