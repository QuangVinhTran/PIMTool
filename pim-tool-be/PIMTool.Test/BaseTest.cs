using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PIMTool.Database;
using PIMTool.Extensions;

namespace PIMTool.Test
{
    public abstract class BaseTest
    {
        protected PimContext Context { get; private set; } = null!;
        protected IServiceProvider ServiceProvider { get; private set; } = null!;


        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            var builder = WebApplication.CreateBuilder();
            services.AddDbContext<PimContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            services.Register();
            ServiceProvider = services.BuildServiceProvider();
            Context = ServiceProvider.GetRequiredService<PimContext>();
        }

        [TearDown]
        public void TearDown()
        {
            Context.Dispose();
        }
    }
}