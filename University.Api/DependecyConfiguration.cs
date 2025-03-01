using University.Application.AllServices;
using University.Application.AllServices.AllServices;
using University.Application.AllServices.ServiceAbstracts;
using University.Data.Data.Repository;
using University.Data.Data.Repository.AllRepositories;
using University.Data.Data.Repository.RepositoryAbstracts;

namespace University.Api
{
    public static class DependencyConfiguration
    {
        public static void RegisterDependencyConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IRepositories, Repostitories>();
            services.AddScoped<IServices, DIServices>();


            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IFacultyRepository, FacultyRepository>();
            services.AddScoped<ILecturerRepository, LecturerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHierarchyRepository, HierarchyRepository>();
            services.AddScoped<ICourseServices, CourseServices>();
            services.AddScoped<IFacultyServices, FacultyServices>();
            services.AddScoped<ILecturerServices,  LecturerServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IHierarchyService, HierarchyServices>();
        }
    }
}
