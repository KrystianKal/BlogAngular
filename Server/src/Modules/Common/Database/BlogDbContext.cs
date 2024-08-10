using BlogBackend.Modules.Articles.Domain;
using BlogBackend.Modules.Profiles;
using BlogBackend.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Common.Database;

public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    private DbSet<ArticleFavorited> ArticleFavoriteds { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        //base.OnModelCreating(modelBuilder);
    }
}
