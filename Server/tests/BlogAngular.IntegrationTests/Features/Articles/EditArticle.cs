using FluentAssertions;
using System.Net;

namespace BlogAngular.IntegrationTests.Features.Articles;

public class EditArticle(ApiWebApplicationFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public async Task Put_Returns_200Ok()
    {
        var request = new
        {
            article = new
            {
                title = "Edited Title"
            }
        };
        var response = await _httpClient.PutAsJsonAsync("api/articles/test-article", request);
        response.Should().HaveStatusCode(HttpStatusCode.OK);

    }
}
