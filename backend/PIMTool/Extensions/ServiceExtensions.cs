using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PIMTool.Core.Constants;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Models.Settings;

namespace PIMTool.Extensions;

public static class ServiceExtensions
{
    
    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            var jwtSettings = config?.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.SigningKey ?? throw new MissingJwtSettingsException())),
                ValidAudience = jwtSettings.Audience,
                ValidIssuer = jwtSettings.Issuer,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    public static void AddAppCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CoreConstants.APP_CORS, builder =>
            {
                var config = services.BuildServiceProvider().GetService<IConfiguration>();
                var allowedOrigins = config?.GetSection("AllowedOrigins").Value ?? "*"; 
                builder.WithOrigins(allowedOrigins.Split(","))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                if (allowedOrigins != "*")
                {
                    builder.AllowCredentials();
                }
                
                builder.Build();
            });
        });
    }
}