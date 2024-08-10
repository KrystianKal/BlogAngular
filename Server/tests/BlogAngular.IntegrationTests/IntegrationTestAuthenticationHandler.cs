using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BlogAngular.IntegrationTests;

public class IntegrationTestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                                  ILoggerFactory logger,
                                                  UrlEncoder encoder,
                                                  TestUserService testUserService)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim (ClaimTypes.NameIdentifier, testUserService.User.UserId.Value.ToString()),
        };
        var identity = new ClaimsIdentity(claims,"IntegrationTest");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "IntegrationTest");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
