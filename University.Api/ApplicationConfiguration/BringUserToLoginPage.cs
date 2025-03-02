using System.Globalization;

namespace University.Api.ApplicationConfiguration;

public static class BringUserToLoginPage
{
    public static void Execute(IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            const string cacheKey = "LogMessage";

            var loggedIn = context.Session.GetString(cacheKey);
            if (string.IsNullOrEmpty(loggedIn) || !DateTime.TryParse(loggedIn, out _))
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
                            context.Session.SetString(cacheKey, DateTime.Now.ToString(CultureInfo.InvariantCulture));
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
    
    private static bool Authenticate(string? username, string? password)
    {
        return username is "admin" && password is "admin";
    }
}