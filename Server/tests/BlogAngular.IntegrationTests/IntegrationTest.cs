using BlogAngular.Modules.Common.Database;
using BlogAngular.Modules.Users;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Respawn.Graph;
using System.Data.Common;

namespace BlogAngular.IntegrationTests;

[Trait("Category", "Integration")]
public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
{
    protected HttpClient _httpClient;
    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;
    private ApiWebApplicationFactory _factory;

    protected IntegrationTest(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
        _dbConnection = new NpgsqlConnection(factory.Configuration.GetConnectionString("Postgres"));
    }

    public async Task InitializeAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            SchemasToInclude = ["public"],
            TablesToIgnore = [new Table("__EFMigrationsHistory")],
            DbAdapter = DbAdapter.Postgres,
            WithReseed = true

        });
        await DataSeeder.SeedDatabase(_factory.Services);
    }

    public async Task DisposeAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
        await DataSeeder.SeedDatabase(_factory.Services);
    }
}
