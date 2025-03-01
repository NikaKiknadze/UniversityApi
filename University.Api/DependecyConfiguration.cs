using University.Application.AllServices;
using University.Application.AllServices.AllServices;
using University.Application.AllServices.ServiceAbstracts;
using University.Data.ContextMethodsDirectory;

namespace University.Api
{
    public static class DependencyConfiguration
    {
        public static void RegisterDependencyConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IServices, DiServices>();
            services.AddScoped<IContextMethods, ContextMethods>();
            services.AddScoped<ICourseServices, CourseServices>();
            services.AddScoped<IFacultyServices, FacultyServices>();
            services.AddScoped<ILecturerServices,  LecturerServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IHierarchyService, HierarchyServices>();
        }
    }
}
