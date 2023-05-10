using DocStorageApi.Data;
using DocStorageApi.Data.Migrations;
using DocStorageApi.Domain.Repository;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.Domain.UnitOfWork;
using DocStorageApi.Identity;
using DocStorageApi.Services;
using DocStorageApi.Services.Interfaces;
using MediatR;

namespace DocStorageApi.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDocStorageApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // .NET ENV
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(configuration);

            // Services responsible by the initial database setup - Handled like that to fit tests db
            services.AddScoped(serviceProvider =>
            {
                var connectionString = configuration.GetConnectionString("DocStorageApiConnection");
                return new DatabaseManager(connectionString);
            });
            services.AddScoped(serviceProvider =>
{
    var connectionString = configuration.GetConnectionString("DocStorageApiConnection");
    return new DbSession(connectionString);
});
            services.AddScoped<DatabaseObjectsManager>();

            // Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddTransient<DocStorageJwtService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAccessControlService, AccessControlService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IS3Service, S3Service>();

            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IAccessGroupRepository, AccessGroupRepository>();
            services.AddTransient<IDocAccessGroupRepository, DocAccessGroupRepository>();
            services.AddTransient<IDocAccessUserRepository, DocAccessUserRepository>();
            services.AddTransient<IUserAccessGroupRepository, UserAccessGroupRepository>();

        }
    }
}
