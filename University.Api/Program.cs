using University.Api.ApplicationConfiguration;
using University.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBuilder();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureApp(app.Environment);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();