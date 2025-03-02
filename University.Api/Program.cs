using Microsoft.EntityFrameworkCore;
using University.Api;
using University.Api.Middlewares;
using University.Application;
using University.Data;

var builder = WebApplication.CreateBuilder(args);

ThreadPool.SetMinThreads(1000, 1000);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = null;
    options.Limits.MaxConcurrentUpgradedConnections = null;
    options.Limits.MaxRequestBodySize = null;
});

builder.Services.AddDbContext<UniversityDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.RegisterDependencyConfiguration();
builder.Services.RegisterPersistenceDependencyConfiguration();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsecrets.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var app = builder.Build();

ApiConfig.Configure(app, app.Environment);

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();