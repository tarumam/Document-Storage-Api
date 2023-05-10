using FluentMigrator.Runner;
using System.Reflection;

namespace DocStorageApi.Data.Migrations
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var databaseService = scope.ServiceProvider.GetRequiredService<DatabaseManager>();

                    //TODO: Might be better to use Use dbup-postgres??
                    var databaseConnection = databaseService.TryCreateDatabase("document_storage_db");

                    if (!string.IsNullOrEmpty(databaseConnection))
                    {
                        var serviceProvider = CreateMigrationService(databaseConnection);
                        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

                        if (runner.HasMigrationsToApplyUp())
                        {
                            runner.ListMigrations();
                            runner.MigrateUp();
                        }

                        var databaseObjects = scope.ServiceProvider.GetRequiredService<DatabaseObjectsManager>();
                        databaseObjects.CreateObjects();
                    }
                }
                catch (Exception ex)
                {
                    //TODO: Log this
                    throw new Exception($"Can't create the database document_storage_db: {ex.Message}");
                }
            }
            return host;
        }
        private static ServiceProvider CreateMigrationService(string connectionString)
        {
            return new ServiceCollection()
                .AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.GetExecutingAssembly())
                .For.Migrations())
                .BuildServiceProvider(false);
        }
    }
}
