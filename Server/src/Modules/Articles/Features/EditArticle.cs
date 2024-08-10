using BlogBackend.Modules.Articles.Exceptions;
using BlogBackend.Modules.Articles.Features.Types;
using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net;
using BlogBackend.Modules.Articles.Utils;

namespace BlogBackend.Modules.Articles.Features;

public record EditArticleRequest(EditArticleRequestData Article);
public record EditArticleRequestData(string? Title, string? Description, string? Body);

[Authorize]
[Route("articles")]
public class EditArticleController(IMediator mediator) : ControllerBase
{
    [HttpPut("{slug}")]
    public async Task<ActionResult<ArticleResponse>> Edit(
        [FromBody] EditArticleRequest request,
        string slug,
        CancellationToken cancellationToken)
        => Ok(await mediator.Send(new EditArticleCommand(request.Article, slug), cancellationToken));
}

public record EditArticleCommand(EditArticleRequestData Article, string Slug) : IRequest<ArticleResponse>;

public class EditArticleCommandValidator : AbstractValidator<EditArticleCommand>
{
    public EditArticleCommandValidator()
    {
        RuleFor(x => x.Article).NotNull();

        RuleFor(x => x.Article.Title)
            .MinimumLength(1)
            .When(x => x.Article.Title is not null);

        RuleFor(x => x.Article.Description)
            .MinimumLength(1)
            .When(x => x.Article.Description is not null);

        RuleFor(x => x.Article.Body)
            .MinimumLength(1)
            .When(x => x.Article.Body is not null);
    }
}

public class EditArticleCommandHandler(IUserAccessor userAccessor, BlogDbContext context, IAuthorService authorService) 
    : IRequestHandler<EditArticleCommand, ArticleResponse>
{
    public async Task<ArticleResponse> Handle(EditArticleCommand request, CancellationToken cancellationToken)
    {
        var currentArticle = await context.Articles
            .SingleOrDefaultAsync(x => x.Slug.Equals(request.Slug));

        if (currentArticle is null)
        {
            throw new ArticleNotFoundException(request.Slug);
        }

        var currentUserIsNotTheAuthor = !currentArticle.AuthorId.Value.ToString().Equals(userAccessor.GetCurrentUserId());
        if (currentUserIsNotTheAuthor)
        {
            throw new CurrentUserIsNotTheAuthorException();
        }

        currentArticle.Title = request.Article.Title ?? currentArticle.Title;
        currentArticle.Description = request.Article.Description ?? currentArticle.Description;
        currentArticle.Body = request.Article.Body ?? currentArticle.Body;
        currentArticle.UpdatedAt = DateTime.UtcNow;

        context.Articles.Update(currentArticle);
        context.SaveChanges();

        var author = await authorService.GetAuthor(currentArticle.AuthorId,cancellationToken);
        return new ArticleResponse(currentArticle, author);
    }
}
