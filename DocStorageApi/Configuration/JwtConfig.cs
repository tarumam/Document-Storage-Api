using DocStorageApi.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace DocStorageApi.Configuration
{
    public static class JwtConfig
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var signingKey = configuration["Jwt:Key"];

            var key = Encoding.ASCII.GetBytes(signingKey);
            services.AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                //options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            services.AddHttpContextAccessor();
        }
    }
}
