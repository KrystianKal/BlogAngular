using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using BlogBackend.Modules.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Users;
using BlogBackend.Modules.Common;

namespace BlogBackend.Modules.Users.Features;

public record LoginData(string Email, string Password);
[Route("users")]
public class LoginController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<UserResponse> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        => await mediator.Send(command, cancellationToken);
}

public record LoginCommand(LoginData User) : IRequest<UserResponse>;
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.User).NotNull();
        RuleFor(x => x.User.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.User.Password).NotEmpty().MinimumLength(3);
    }
}
public class LoginCommandHandler(IHttpContextAccessor contextAccessor, BlogDbContext context) : IRequestHandler<LoginCommand, UserResponse>
{
    public async Task<UserResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.User.Email, cancellationToken);
        if (user == null)
        {
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, new { user = "Does not exist." });
        }

        var passwordIsCorrect = Crypto.VerifyHashedPassword(user.Password, request.User.Password);
        if (!passwordIsCorrect)
        {
            throw new ApiException(System.Net.HttpStatusCode.BadRequest, new { password = "invalid" });
        }

        await contextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(
                new ClaimsIdentity(
                    [
                       new Claim(ClaimTypes.NameIdentifier, user.UserId.Value.ToString())
                    ],
                    CookieAuthenticationDefaults.AuthenticationScheme)
                ),
                new AuthenticationProperties()
                {
                    IsPersistent = true,
                });
        return new UserResponse(user.Email, user.Name);
    }
}
