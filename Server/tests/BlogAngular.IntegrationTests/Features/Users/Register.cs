using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace BlogAngular.IntegrationTests.Features.Users;

public class Register(ApiWebApplicationFactory factory) : IntegrationTest(factory)
{

    [Fact]
    public async Task Post_Returns_201Created()
    {
        var content = new
        {
            user = new
            {
                username = "TestUser",
                email = "mail@mail.com",
                password = "password123"
            }
        };

        var response = await _httpClient.PostAsJsonAsync("/api/users",content);
        response.Should().HaveStatusCode(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_InvalidEmail_Returns_422UnprocessableEntity()
    {
        var content = new
        {
            user = new
            {
                username = "TestUser",
                email = "mailmail.com",
                password = "password123"
            }
        };
        var response = await _httpClient.PostAsJsonAsync("/api/users",content);
        response.Should().HaveStatusCode(HttpStatusCode.UnprocessableEntity);
    }
}
