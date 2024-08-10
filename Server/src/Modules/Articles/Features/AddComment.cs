using BlogBackend.Modules.Articles.Domain;
using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

public record AddCommentRequest(string Body);

[Authorize]
[Route("articles")]
public class AddCommentController(IMediator mediator) : ControllerBase
{
    [HttpPost("{slug}/comments")]
    public async Task<ActionResult<CommentResponse>> Post(string slug, [FromBody] AddCommentRequest comment, CancellationToken cancellationToken)
        => Created("", await mediator.Send(new AddCommentCommand(comment, slug), cancellationToken));
}

public record AddCommentCommand(AddCommentRequest Comment, string Slug) : IRequest<CommentResponse>;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.Comment).NotNull();
        RuleFor(x => x.Comment.Body).NotEmpty();
    }
}

public class AddCommentCommandHandler(BlogDbContext context, IUserAccessor userAccessor, IAuthorService authorService)
    : IRequestHandler<AddCommentCommand, CommentResponse>
{
    public async Task<CommentResponse> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var authorId = Guid.Parse(userAccessor.GetCurrentUserId()!);

        var comment = new Comment
        {
            Body = request.Comment.Body,
            AuthorId = authorId
        };

        var article = await context.Articles.SingleOrDefaultAsync(x => x.Slug.Equals(request.Slug));

        if (article is null)
        {
            throw new ArticleNotFoundException(request.Slug);
        }


        article.Comments.Add(comment);
        context.Articles.Update(article);
        await context.SaveChangesAsync(cancellationToken);

        var author =  await authorService.GetAuthor(comment.AuthorId, cancellationToken);
        return new CommentResponse(comment, author);
    }
}
