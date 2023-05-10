namespace DocStorageApi.Configuration
{
    public static class ApiConfig
    {
        public static void AddDocStorageCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        public static void MapDocStorageControllers(this WebApplication app)
        {
            app.MapControllers();
            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
        }
    }
}
