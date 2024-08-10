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
public class UnfollowController(IMediator mediator) : ControllerBase
{
    [HttpDelete("{userName}/follow")]
    public async Task<ActionResult<ProfileResponse>> Unollow(string userName, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new UnfollowCommand(userName), cancellationToken));
}
public record UnfollowCommand(string UserName) : IRequest<ProfileResponse>;

public class UnfollowCommandHandler(BlogDbContext context, IUserAccessor userAccessor)
    : IRequestHandler<UnfollowCommand, ProfileResponse>
{
    public async Task<ProfileResponse> Handle(UnfollowCommand request, CancellationToken cancellationToken)
    {
        var profileToUnfollow = await ProfileHelper.GetUserProfile(request.UserName, context, cancellationToken);
        var currentUserId = userAccessor.GetCurrentUserId()!;

        if (profileToUnfollow.UserId == UserId.Parse(currentUserId))
        {
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,
                new { Profile = "Cannot unfollow itself" });
        }

        var currentUserProfile = await context.Profiles
            .Include(x => x.Following)
            .SingleOrDefaultAsync(x => x.UserId == UserId.Parse(currentUserId), cancellationToken);

        if (currentUserProfile is null)
        {
            throw new ProfileNotFoundException(currentUserId);
        }

        var profileFollow = currentUserProfile.Following
            .SingleOrDefault(x => x.FollowingId == profileToUnfollow.ProfileId);

        if (profileFollow is null)
        {
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,
                new { Profile = "Profile to unfollow is not being followed" });
        }

        currentUserProfile.Following.Remove(profileFollow);
        context.Profiles.Update(currentUserProfile);
        await context.SaveChangesAsync(cancellationToken);
        return ProfileResponse.From(profileToUnfollow, false);

    }
}
