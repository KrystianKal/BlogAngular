using Microsoft.EntityFrameworkCore;

namespace BlogBackend.Modules.Common.Database;

public static class Extensions
{
    public static void EnsureDatabaseCreated(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        (scope.ServiceProvider.GetService(typeof(BlogDbContext)) as DbContext)
            ?.Database.Migrate();
    }

    public static void AddPostgres<T>(this IServiceCollection services, ConfigurationManager configuration) where T : DbContext
    {

        services.AddDbContext<T>(o =>
            o.UseNpgsql(configuration.GetConnectionString("postgres"))
        );
    }
}
