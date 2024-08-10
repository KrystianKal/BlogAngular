using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Profiles.Exceptions;
using BlogBackend.Modules.Profiles.Utils;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Profiles.Features;


[Authorize]
[Route("profiles")]
public class UploadProfilePictureController(IMediator mediator) : ControllerBase
{
    [HttpPost("picture")]
    public async Task<ActionResult<string>> Upload([FromForm] UploadProfilePictureCommand command, CancellationToken cancellationToken)
        => Created("", await mediator.Send(command, cancellationToken));
}

public record UploadProfilePictureCommand(IFormFile File) : IRequest<string>;
public class UploadProfilePictureCommandValidator : AbstractValidator<UploadProfilePictureCommand>
{
    private static readonly string[] PermitedExtensions = [".jpg", ".png", ".jpeg"];
    public UploadProfilePictureCommandValidator()
    {
        RuleFor(x=>x.File).NotEmpty();

        RuleFor(x => x.File)
            .Must(HaveValidExtension).WithMessage($"Invalid file format. Only allowed: {String.Join(',', PermitedExtensions)}")
            .When(x => x.File is not null);
            ;
    }

    private bool HaveValidExtension(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        return PermitedExtensions.Contains(ext);
    }
    private bool BeAValidBase64String(string base64Image)
    {
        try
        {
            Convert.FromBase64String(base64Image);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
public class UploadProfilePictureCommandHandler(BlogDbContext context,IUserAccessor userAccessor, IImageService imageService) 
    : IRequestHandler<UploadProfilePictureCommand, string>
{
    public async Task<string> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        //get current user
        var currentUserId = userAccessor.GetCurrentUserId()!;
        var currentUserProfile = await context.Profiles
            .SingleOrDefaultAsync(x => x.UserId.Equals(UserId.Parse(currentUserId)), cancellationToken);
        if (currentUserProfile == null)
        {
            throw new ProfileNotFoundException(currentUserId);
        }

        if (currentUserProfile.Image != null)
        {
            imageService.Delete(currentUserProfile.Image);
        }


        var fileUrl = await imageService.Upload(request.File, cancellationToken);

        currentUserProfile.Image = new ProfileImage(fileUrl);

        context.Profiles.Update(currentUserProfile);
        await context.SaveChangesAsync(cancellationToken);
        return fileUrl;
    }
}
