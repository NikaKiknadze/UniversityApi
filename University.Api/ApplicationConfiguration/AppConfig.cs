using Swashbuckle.AspNetCore.SwaggerUI;

namespace University.Api.ApplicationConfiguration;

public static class AppConfig
{
    public static void ConfigureApp(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders();

        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseSession();
        app.UseStaticFiles();
        app.UseSwagger();

        if (!env.IsDevelopment())
            BringUserToLoginPage.Execute(app);

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
                    context.Response.Redirect("/swagger/login.html");

                await context.Response.WriteAsync("Service Is Up & Running...");
            });
        });
    }
}