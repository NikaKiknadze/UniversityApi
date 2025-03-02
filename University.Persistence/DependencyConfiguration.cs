using University.Data.ContextMethodsDirectory;

namespace University.Data;

public static class DependencyConfiguration
{
    public static void RegisterPersistenceDependencyConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IContextMethods, ContextMethods>();
        services.AddScoped<IUniversityContext, UniversityContext>();
    }
}