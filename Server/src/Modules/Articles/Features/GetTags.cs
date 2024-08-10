using BlogBackend.Modules.Articles.Domain;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Articles.Features;

public record TagsResponse(string[] Tags);

[Route("tags")]
public class GetTagsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<TagsResponse> Get()
        => await mediator.Send(new GetTagsQuery());
}

public record GetTagsQuery : IRequest<TagsResponse>;

public class GetTagsQueryHandler(BlogDbContext context) 
    : IRequestHandler<GetTagsQuery, TagsResponse>
{
    public async Task<TagsResponse> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        => new TagsResponse(
            await context.Tags.Select(x => x.Name)
            .ToArrayAsync(cancellationToken)
            );
}
