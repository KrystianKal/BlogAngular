using BlogBackend.Modules.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogBackend.Modules.Users.Features;

[Authorize]
[Route("users")]
public class LogoutController (IMediator mediator)
    : ControllerBase
{
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new LogoutCommand(), cancellationToken));
}

public record LogoutCommand() : IRequest<Unit>;
public class LogoutCommandHandler(IHttpContextAccessor httpContextAccessor) 
    : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await httpContextAccessor.HttpContext!
            .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Unit.Value;
    }
}
