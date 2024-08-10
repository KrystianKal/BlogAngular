using BlogBackend.Modules.Common.Database;
using MediatR;

namespace BlogBackend.Modules.Profiles.Shared;

public record CreateProfileCommand(Guid UserId, string Name) : IRequest<bool>;
public class CreateProfileCommandHandler(BlogDbContext context)
    : IRequestHandler<CreateProfileCommand, bool>
{
    public async Task<bool> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = new Profile
        {
            ProfileName = request.Name,
            UserId = request.UserId
        };
        await context.Profiles.AddAsync(profile, cancellationToken);
        await context.SaveChangesAsync();
        return true;
    }
}
