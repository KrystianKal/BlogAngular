using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Profiles.Exceptions;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogBackend.Modules.Profiles.Utils;

namespace BlogBackend.Modules.Profiles.Features;

[Authorize]
[Route("profiles")]
public class FollowController(IMediator mediator) : ControllerBase
{
    [HttpPost("{userName}/follow")]
    public async Task<ActionResult<ProfileResponse>> Follow(string userName, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new FollowCommand(userName), cancellationToken));
}

public record FollowCommand(string UserName) : IRequest<ProfileResponse>;

public class FollowCommandHandler(BlogDbContext context, IUserAccessor userAccessor) : IRequestHandler<FollowCommand, ProfileResponse>
{
    public async Task<ProfileResponse> Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        var profileToFollow = await ProfileHelper.GetUserProfile(request.UserName, context, cancellationToken);
        var currentUserId = userAccessor.GetCurrentUserId()!;

        if (profileToFollow.UserId == UserId.Parse(currentUserId))
        {
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, new { Profile = "Cannot follow itself" });
        }

        var currentUserProfile = await context.Profiles.SingleOrDefaultAsync(x => x.UserId == UserId.Parse(currentUserId), cancellationToken);
        if (currentUserProfile is null)
        {
            throw new ProfileNotFoundException(currentUserId);
        }

        profileToFollow.Followers.Add(new ProfileFollow(currentUserProfile, profileToFollow));
        context.Profiles.Update(profileToFollow);
        await context.SaveChangesAsync(cancellationToken);
        return ProfileResponse.From(profileToFollow, true);

    }
}
