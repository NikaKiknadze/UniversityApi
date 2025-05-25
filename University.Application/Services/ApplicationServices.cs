using University.Application.Services.Auth;
using University.Application.Services.Courses;
using University.Application.Services.Excel;
using University.Application.Services.Faculties;
using University.Application.Services.Identity;
using University.Application.Services.Lecturers;
using University.Application.Services.Users;
using University.Data.ContextMethodsDirectory;

namespace University.Application.Services;

public static class DependencyConfiguration
{
    public static void RegisterDependencyConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ICourseServices, CourseServices>();
        services.AddScoped<IFacultyServices, FacultyServices>();
        services.AddScoped<ILecturerServices,  LecturerServices>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IUniversityContext, UniversityContext>();
        services.AddScoped<IUniversityContext, UniversityContext>();
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IUserIdentity, UserIdentity>();
        services.AddScoped<IExcelServices, ExcelServices>();
    }
}