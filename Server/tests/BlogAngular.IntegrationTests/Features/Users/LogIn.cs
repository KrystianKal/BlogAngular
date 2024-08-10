using FluentAssertions;

namespace BlogAngular.IntegrationTests.Features.Users;

public class LogIn(ApiWebApplicationFactory factory, TestUserService testUserService) : IntegrationTest(factory)
{
    [Fact]
    public async Task Post_Returns_200Ok()
    {
        var request = new
        {
            user = new
            {
                email = testUserService.User.Email.Value,
                password = testUserService.User.Password,
            }
        };
        var response = await _httpClient.PostAsJsonAsync("/api/users/login",request);
        response.Headers.Any( x => x.Key == "Set-Cookie").Should().BeTrue();
    }
}
