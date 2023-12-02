using UniversityApi.Service.ServiceAbstracts;
using UniversityApi.Service.Services;

namespace UniversityApi.Service
{
    public class DIServices : IServices
    {
        public DIServices(ICourseServices courseServices,
                          IFacultyServices facultyServices,
                          ILecturerServices lecturerServices,
                          IUserServices userServices)
        {
            CourseServices = courseServices;
            FacultyServices = facultyServices;
            LecturerServices = lecturerServices;
            UserServices = userServices;
        }
        public ICourseServices CourseServices { get; }
        public IFacultyServices FacultyServices { get; }
        public ILecturerServices LecturerServices { get; }
        public IUserServices UserServices { get; }

    }
}
