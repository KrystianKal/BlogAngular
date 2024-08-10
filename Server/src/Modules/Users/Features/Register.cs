using BlogBackend.Modules.Common.Exceptions;
using BlogBackend.Modules.Profiles.Shared;
using BlogBackend.Modules.Common;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Users;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Users.Features;

public record RegisterRequest(string Username, string Email, string Password);

[Route("users")]
public class Register(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserResponse>> Post([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        return Created("", await mediator.Send(command, cancellationToken));
    }
}

public record RegisterCommand(RegisterRequest user) : IRequest<UserResponse>;
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.user).NotNull().NotEmpty();
        RuleFor(x => x.user.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.user.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.user.Password).NotEmpty().MinimumLength(3);
    }
}
public class RegisterCommandHandler(BlogDbContext context, IMediator mediator)
    : IRequestHandler<RegisterCommand, UserResponse>
{
    public async Task<UserResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userEmail = new Email(request.user.Email);

        var emailAlreadyTaken = await context.Users.AsNoTracking()
            .AnyAsync(x => x.Email == userEmail, cancellationToken);
        var nameAlreadyTaken = await context.Users.AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == request.user.Username.ToLower(), cancellationToken);

        Dictionary<string,string> errors = new();
        if (emailAlreadyTaken)
        {
            errors.Add("email",  "Already taken");
        }
        if (nameAlreadyTaken)
        {
            errors.Add("name",  "Already taken");
        }
        if(errors.Any()){
            throw new ApiException(System.Net.HttpStatusCode.BadRequest,  errors );
        }


        var user = new User
        {
            Name = request.user.Username,
            Email = userEmail,
            Password = Crypto.HashPassword(request.user.Password)
        };

        await context.Users.AddAsync(user);

        var profileCreated = await mediator.Send(new CreateProfileCommand(user.UserId, user.Name));

        if (profileCreated)
        {
            await context.SaveChangesAsync();
        }
        else
        {
            throw new ApiException(System.Net.HttpStatusCode.InternalServerError
                , new { Profile = "Could not be created." });
        }


        return new UserResponse(user.Email, user.Name);
    }
}
