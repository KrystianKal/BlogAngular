using BlogBackend.Modules.Articles.Domain;
using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

[Route("articles")]
public class GetComments(IMediator mediator) : ControllerBase
{
    [HttpGet("{slug}/comments")]
    public async Task<ActionResult<CommentsResponse>> Get(string slug, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetCommentsQuery(slug), cancellationToken));

    [HttpGet("{slug}/comments/a")]
    public async Task<dynamic> GetA(string slug, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetCommentsQuery(slug), cancellationToken));
}

public record GetCommentsQuery(string Slug) : IRequest<CommentsResponse>;

public class GetCommentsQueryHandler(BlogDbContext context, IAuthorService authorService)
    : IRequestHandler<GetCommentsQuery, CommentsResponse>
{
    public async Task<CommentsResponse> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var article = await context.Articles
            .Include(x => x.Comments)
            .SingleOrDefaultAsync(x => x.Slug.Equals(request.Slug), cancellationToken);

        if (article is null)
        {
            throw new ArticleNotFoundException(request.Slug);
        }

        var commentResponses = new List<CommentResponse>();
        foreach(var comment in article.Comments){
            var author =  await authorService.GetAuthor(comment.AuthorId, cancellationToken);
            commentResponses.Add(new CommentResponse(comment,author));
        }
        return new CommentsResponse(commentResponses.OrderByDescending(x=>x.CreatedAt).ToArray());
    }
}
