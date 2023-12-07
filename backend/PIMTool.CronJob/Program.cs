using Hangfire;
using PIMTool.Core.Models.Request;
using PIMTool.CronJob;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHangfire(config => 
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangFireConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<TestService>();

var app = builder.Build();

app.MapGet("/api/add-job", () =>
{
    RecurringJob.AddOrUpdate("EasyJob", () => Console.WriteLine("Hello, World!"), Cron.Minutely());
    RecurringJob.RemoveIfExists("EasyJob");
    
    return Results.Ok("Job added");
});



app.UseStaticFiles();
app.UseHangfireDashboard("");
using (var scope = app.Services.CreateScope())
{
    var test = scope.ServiceProvider.GetService<TestService>();
    var request = new CreateProjectRequest()
    {

    };
    RecurringJob.AddOrUpdate("Something", () => test!.something(), Cron.Daily());
}
app.Run();

