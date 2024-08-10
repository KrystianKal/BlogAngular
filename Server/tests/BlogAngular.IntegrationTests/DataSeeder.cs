using BlogAngular.Modules.Users;
using BlogBackend.Modules.Articles.Domain;
using BlogBackend.Modules.Common.Database;
using BlogBackend.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace BlogAngular.IntegrationTests;

public static class DataSeeder
{
    internal static async Task SeedDatabase(IServiceProvider services)
    {
        await SeedTestUser(services);
        await SeedArticles(services);
    }

    private static async Task SeedTestUser(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        var testUserService = scope.ServiceProvider.GetRequiredService<TestUserService>();

        // Check if the test user already exists
        var testUser = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == "test@mail.com");

        if (testUser is not null)
        {
            testUserService.User = testUser;
            return;
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Name = "testuser",
            Email = "test@mail.com",
            Password = Crypto.HashPassword("test")
        };

        testUserService.User = user;

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }


    private static async Task SeedArticles(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        var testUserService = scope.ServiceProvider
            .GetRequiredService<TestUserService>()
            ?? throw new Exception("testUserService not found");


        // Check if the article already exists
        var testArticle = await dbContext.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Slug == "test-article");

        if (testArticle is not null)
        {
            return;
        }

        var Tags = new List<Tag>() {
            new("TestTag1"),
            new("TestTag2")
        };

        var article = new Article
        {
            AuthorId = testUserService.User.UserId,
            Title = "Test article",
            Description = "Article description",
            Body = "Test article body",
            Tags = Tags
        };

        await dbContext.Articles.AddAsync(article);

        await dbContext.SaveChangesAsync();
    }
}