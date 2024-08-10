using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Profiles.Exceptions;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using BlogBackend.Modules.Profiles.Utils;

namespace BlogBackend.Modules.Profiles.Features;

[Route("profiles")]
public class GetProfileController(IMediator mediator) : ControllerBase
{
    [HttpGet("{userName}")]
    public async Task<ActionResult<ProfileResponse>> Get(string userName, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetProfileQuery(userName), cancellationToken));
}

public record GetProfileQuery(string UserName) : IRequest<ProfileResponse>;

public class GetProfileQueryHandler(BlogDbContext context, IUserAccessor userAccessor) : IRequestHandler<GetProfileQuery, ProfileResponse>
{
    public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await ProfileHelper.GetUserProfile(request.UserName, context, cancellationToken);

        var currentUserId = userAccessor.GetCurrentUserId();

        if (currentUserId is null || profile.UserId == UserId.Parse(currentUserId))
        {
            return ProfileResponse.From(profile);
        }

        var isFollowing = await context.Profiles.AsNoTracking()
            .Include(x => x.Following)
            .AnyAsync(p => p.UserId == UserId.Parse(currentUserId)
            && p.Following.Any(f => f.FollowingId == profile.ProfileId));


        return ProfileResponse.From(profile, isFollowing);
    }
}
