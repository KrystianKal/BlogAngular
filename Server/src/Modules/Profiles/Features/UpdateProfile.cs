using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Profiles.Exceptions;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Profiles.Utils;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogBackend.Modules.Profiles.Features;

public record UpdateProfileRequest(string? Name, string? Bio, string? Image);
[Authorize]
[Route("profiles")]
public class UpdateProfileController(IMediator mediator) : ControllerBase
{
    [HttpPatch]
    public async Task<ActionResult<ProfileResponse>> Update( [FromBody] UpdateProfileRequest Profile,
                                                            CancellationToken cancellationToken)
        => Ok(await mediator.Send(new UpdateProfileCommand(Profile), cancellationToken));
}

public record UpdateProfileCommand(UpdateProfileRequest Profile) : IRequest<ProfileResponse>;

public class UpdateProfileCommandValidator
    : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Profile).NotNull();

        RuleFor(x => x.Profile.Name)
            .NotEmpty()
            .When(x=>x.Profile.Name is not null);

        RuleFor(x => x.Profile.Bio)
            .NotEmpty()
            .When(x=>x.Profile.Bio is not null);

        RuleFor(x => x.Profile.Image)
            .Url()
            .When(x => x.Profile.Image is not null);
    }
}

public class UpdateProfileCommandHandler(BlogDbContext context,IImageService imageService, IUserAccessor userAccessor)
    : IRequestHandler<UpdateProfileCommand, ProfileResponse>
{
    public async Task<ProfileResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = userAccessor.GetCurrentUserId()!;
        var profile = await context.Profiles
            .SingleOrDefaultAsync(x => x.UserId.Equals(UserId.Parse(currentUserId)), cancellationToken);
        if (profile == null)
        {
            throw new ProfileNotFoundException(currentUserId);
        }

        profile.ProfileName = request.Profile.Name ?? profile.ProfileName;
        profile.Bio = request.Profile.Bio ?? profile.Bio;
        if(request.Profile.Image is not null)
        {
            imageService.Delete(profile.Image!);
            profile.Image = new ProfileImage( request.Profile.Image);
        }

        context.Profiles.Update(profile);
        await context.SaveChangesAsync(cancellationToken);

        return ProfileResponse.From(profile);
    }
}
