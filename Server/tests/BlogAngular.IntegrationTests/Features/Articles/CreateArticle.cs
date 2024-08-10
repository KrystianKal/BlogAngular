using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace BlogAngular.IntegrationTests.Features.Articles;

public class CreateArticle(ApiWebApplicationFactory factory)
    :IntegrationTest(factory)
{
    [Fact]
    public async Task Post_Returns_201Created()
    {
        var content = new
        {
            article = new
            {
                title = "New article",
                description = "Test description",
                body = "Test body",
                tagList = new[] { "testTag", "testTag2", "testTag3" }
            }
        };
        var response = await _httpClient.PostAsJsonAsync("/api/articles", content);
        response.Should().HaveStatusCode(HttpStatusCode.Created);
    }
}
