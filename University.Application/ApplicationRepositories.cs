namespace University.Application;

public static class ApplicationRepositories
{
    public static void RegisterRepositoriesDependencyConfiguration(this IServiceCollection services)
    {
        services.AddScoped(typeof(Data.Repositories.Interfaces.IGenericRepository<>), typeof(Data.Repositories.GenericRepository<>));
        services.AddScoped<Data.Repositories.Interfaces.IUserRepository, Data.Repositories.UserRepository>();
        services.AddScoped<Data.Repositories.Interfaces.IFacultyRepository, Data.Repositories.FacultyRepository>();
        services.AddScoped<Data.Repositories.Interfaces.ICourseRepository, Data.Repositories.CourseRepository>();
        services.AddScoped<Data.Repositories.Interfaces.ILecturerRepository, Data.Repositories.LecturerRepository>();
    }
}