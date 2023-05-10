using DocStorageApi.Data;
using DocStorageApi.Data.Migrations;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DocStorageApi.DbObjects.Test.Config
{
    public class DocStorageAppFactory<T> : WebApplicationFactory<T> where T : class
    {
        //TODO: Update this
        private readonly PostgreSqlTestcontainer _dbContainer =
            new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "document_storage_db",
                Username = "postgres",
                Password = "abc@123",
            })
            .WithImage("postgres:latest")
            .Build();

        public DocStorageAppFactory()
        {
            _dbContainer.StartAsync().Wait();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbSession>();
                services.AddScoped(_ => new DbSession(_dbContainer.ConnectionString));

                services.RemoveAll<DatabaseManager>();
                services.AddScoped(_ => new DatabaseManager(_dbContainer.ConnectionString));
            });
        }
    }
}
