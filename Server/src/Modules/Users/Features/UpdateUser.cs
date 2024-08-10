using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Users;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Users.Features;

public record UpdateUserRequest(string? Email, string? Username, string? Password);

[Authorize]
[Route("users")]
public class UpdateUserController(IMediator mediator) : ControllerBase
{
    [HttpPatch]
    public async Task<UserResponse> Update([FromBody] UpdateUserRequest request, CancellationToken token)
        => await mediator.Send(new UpdateUserCommand(request), token);
}

public record UpdateUserCommand(UpdateUserRequest User) : IRequest<UserResponse>;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.User).NotNull().NotEmpty();
        RuleFor(x => x.User.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.User.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.User.Password).NotEmpty().MinimumLength(3);
    }
}

public class UpdateUserCommandHandler(BlogDbContext context, IUserAccessor userAccessor)
    : IRequestHandler<UpdateUserCommand, UserResponse>
{
    public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = userAccessor.GetCurrentUserId()!;
        var currentUser = await context.Users.SingleOrDefaultAsync(x => x.UserId == UserId.Parse(currentUserId));
        if (currentUser == null)
        {
            throw new ApiException(System.Net.HttpStatusCode.InternalServerError,
                new { User = "not found." });
        }

        currentUser.Email = request.User.Email ?? currentUser.Email;
        currentUser.Name = request.User.Username ?? currentUser.Name;
        currentUser.Password = request.User.Password is not null
            ? Crypto.HashPassword(request.User.Password)
            : currentUser.Password;
        context.Users.Update(currentUser);
        await context.SaveChangesAsync(cancellationToken);

        return new UserResponse(currentUser.Email, currentUser.Name);
    }
}

