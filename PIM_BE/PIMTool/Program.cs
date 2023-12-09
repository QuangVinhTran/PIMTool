using Microsoft.EntityFrameworkCore;
using PIMTool.Database;
using PIMTool.Extensions;
using PIMTool.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<PimContext>(options => options.UseSqlServer("server =(local); database=PIM;uid=sa;pwd=1234567890;Trustservercertificate=true"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Register();
builder.Services.ConfigurationService();

var app = builder.Build();

EnsureMigrate(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors();

app.MapControllers();

app.Run();



void EnsureMigrate(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var pimContext = scope.ServiceProvider.GetRequiredService<PimContext>();
    pimContext.Database.Migrate();
}