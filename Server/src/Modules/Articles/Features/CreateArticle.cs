using BlogBackend.Modules.Articles.Domain;
using BlogBackend.Modules.Articles.Utils;
using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

public record CreateArticleRequest(string Title, string Description, string Body, string[]? TagList);

[Authorize]
[Route("articles")]
public class CreateArticleController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ArticleResponse>> Create([FromBody] CreateArticleCommand command, CancellationToken cancellationToken)
        => Created("", await mediator.Send(command, cancellationToken));
};

public record CreateArticleCommand(CreateArticleRequest Article) : IRequest<ArticleResponse>;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Article).NotNull();
        RuleFor(x => x.Article.Title).NotNull().NotEmpty();
        RuleFor(x => x.Article.Description).NotNull().NotEmpty();
        RuleFor(x => x.Article.Body).NotNull().NotEmpty();
        RuleFor(x => x.Article.TagList).NotEmpty();
    }
}

public class CreateArticleCommandHandler(BlogDbContext context, IUserAccessor userAccessor, IAuthorService authorService) 
    : IRequestHandler<CreateArticleCommand, ArticleResponse>
{
    public async Task<ArticleResponse> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var authorId = Guid.Parse(userAccessor.GetCurrentUserId()!);


        var article = new Article
        {
            AuthorId = authorId,
            Title = request.Article.Title,
            Body = request.Article.Body,
            Description = request.Article.Description,
        };

        var articleAlreadyExists = context.Articles.Any(x => x.Slug.Equals(article.Slug));

        if (articleAlreadyExists)
        {
            throw new ApiException(
                System.Net.HttpStatusCode.BadRequest,
                new { article = $"Article with slug: \"{article.Slug}\"  already exists" }
                );
        }

        var articleTags = request.Article.TagList;
        if (articleTags?.Length > 0)
        {
            var existingTags = await context.Tags
                .Where(x => articleTags.Contains(x.Name))
                .ToListAsync(cancellationToken);

            var newTags = articleTags
                .Except(existingTags.Select(t => t.Name))
                .Select(tagName => new Tag(tagName))
                .ToList();

            article.Tags.AddRange(existingTags);
            article.Tags.AddRange(newTags);

        }

        await context.Articles.AddAsync(article, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var author = await authorService.GetAuthor(authorId,cancellationToken);
        return new ArticleResponse(article, author);

    }

}
