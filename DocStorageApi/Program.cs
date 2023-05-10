using DocStorageApi.Configuration;
using DocStorageApi.Data.Migrations;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddDocStorageApiServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddOpenApiDocumentation();
builder.Services.AddDocStorageCors();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

app.MigrateDatabase();
app.UseHttpsRedirection();
app.UseOpenApiDocumentation();
app.UseAuthentication();
app.UseAuthorization();
app.MapDocStorageControllers();

app.Run();

// Used for right reference to program.cs
public partial class Program { }