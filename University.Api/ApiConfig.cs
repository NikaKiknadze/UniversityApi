using System.Globalization;
using Swashbuckle.AspNetCore.SwaggerUI;
using static System.DateTime;

namespace University.Api;

public static class ApiConfig
{
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSession();
        app.UseStaticFiles();
        app.UseSwagger();

        if (!env.IsDevelopment())
        {
            app.Use(async (context, next) =>
            {
                const string cacheKey = "LogMessage";

                var loggedIn = context.Session.GetString(cacheKey);
                if (string.IsNullOrEmpty(loggedIn) || !TryParse(loggedIn, out _))
                {
                    if (context.Request.Path.StartsWithSegments("/swagger") &&
                        !context.Request.Path.StartsWithSegments("/swagger/login.html"))
                    {
                        context.Response.Redirect("/swagger/login.html");
                        return;
                    }

                    if (context.Request.Path.StartsWithSegments("/swagger/login.html"))
                    {
                        if (context.Request.Method == "POST")
                        {
                            var username = context.Request.Form["username"];
                            var password = context.Request.Form["password"];

                            if (Authenticate(username, password))
                            {
                                context.Session.SetString(cacheKey, Now.ToString(CultureInfo.InvariantCulture));
                                context.Response.Redirect("/swagger/index.html");
                                return;
                            }

                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Invalid username or password");
                            return;
                        }

                        await context.Response.SendFileAsync("wwwroot/swagger/login.html");
                        return;
                    }
                }

                await next();
            });
        }

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "UniversityApi");
            options.RoutePrefix = "swagger";
            options.DisplayRequestDuration();
            options.DocExpansion(DocExpansion.None);
            options.DefaultModelsExpandDepth(-1);
        });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.MapGet("/", async context =>
            {
                if (env.IsDevelopment())
                {
                    context.Response.Redirect("/swagger/login.html");
                }

                await context.Response.WriteAsync("Service Is Up & Running...");
            });
        });
    }


    private static bool Authenticate(string? username, string? password)
    {
        return username == "admin" && password == "admin";
    }
}