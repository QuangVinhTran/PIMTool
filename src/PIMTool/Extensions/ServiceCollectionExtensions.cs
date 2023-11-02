using PIMTool.Core.Interfaces.Repositories;
using PIMTool.Core.Interfaces.Services;
using PIMTool.Database;
using PIMTool.Repositories;
using PIMTool.Services;

namespace PIMTool.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IProjectEmployeeService, ProjectEmployeeService>();
        }
    }
}