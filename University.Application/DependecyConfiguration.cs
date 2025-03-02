using University.Application.Services.Courses;
using University.Application.Services.Faculties;
using University.Application.Services.Lecturers;
using University.Application.Services.Users;

namespace University.Application
{
    public static class DependencyConfiguration
    {
        public static void RegisterDependencyConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ICourseServices, CourseServices>();
            services.AddScoped<IFacultyServices, FacultyServices>();
            services.AddScoped<ILecturerServices,  LecturerServices>();
            services.AddScoped<IUserServices, UserServices>();
        }
    }
}
