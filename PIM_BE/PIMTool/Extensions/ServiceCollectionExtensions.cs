using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PIMTool.Core.Domain.Entities;
using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Repositories;
using PIMTool.Services;
using System.Text;

namespace PIMTool.Extensions;

public static class ServiceCollectionExtensions
{
    public static void Register(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IGroupSevice, GroupService>();
        services.AddScoped<IProjectEmployeesService, ProjectEmployeesService>();
    }
    public static void ConfigurationService(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;
        //services.AddDbContext<TestJwtContext>(options =>
        //{
        //    options.UseSqlServer(configuration.GetConnectionString("SolidEcommerceDbConn"), sqlOptions => sqlOptions.CommandTimeout(60));
        //});

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:ValidIssuer"],
            ValidAudience = configuration["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"])),
            ClockSkew = TimeSpan.Zero
        };
    });
        services.AddMvc();
    }
}