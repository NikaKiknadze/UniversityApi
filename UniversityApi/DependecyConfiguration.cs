using UniversityApi.Repository.Repositoryes;
using UniversityApi.Repository.RepositoryAbstracts;
using UniversityApi.Repository;
using UniversityApi.Service.ServiceAbstracts;
using UniversityApi.Service.Services;
using UniversityApi.Service;

namespace UniversityApi
{
    public static class DependecyConfiguration
    {
        public static IServiceCollection RegisterDependencyConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IRepositories, Repostitories>();
            services.AddScoped<IServices, DIServices>();


            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IFacultyRepository, FacultyRepository>();
            services.AddScoped<ILecturerRepository, LecturerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseServices, CourseServices>();
            services.AddScoped<IFacultyServices, FacultyServices>();
            services.AddScoped<ILecturerServices,  LecturerServices>();
            services.AddScoped<IUserServices, UserServices>();


            return services;
        }
    }
}
