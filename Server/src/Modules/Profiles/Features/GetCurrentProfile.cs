using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Profiles.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Profiles.Features;

[Authorize]
[Route("profiles")]
public class GetCurrentProfileController(IMediator mediator)
    : ControllerBase
{
    [HttpGet]
    public async Task<ProfileResponse> Get(CancellationToken token)
        => await mediator.Send(new GetCurrentProfileQuery(), token);
}

public record GetCurrentProfileQuery : IRequest<ProfileResponse>;

public class GetCurrentProfileQueryHandler(BlogDbContext context, IUserAccessor userAccessor)
    : IRequestHandler<GetCurrentProfileQuery, ProfileResponse>
{
    public async Task<ProfileResponse> Handle(GetCurrentProfileQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await userAccessor.GetCurrentUser(cancellationToken);
        var currentUserProfile = await context.Profiles
            .SingleOrDefaultAsync(x => x.UserId.Equals(currentUser.UserId), cancellationToken);
        ProfileNotFoundException.ThrowIfNull(currentUserProfile, currentUser.UserId);
        return ProfileResponse.From(currentUserProfile);
    }
}
