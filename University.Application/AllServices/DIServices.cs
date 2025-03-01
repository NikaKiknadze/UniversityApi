using University.Application.AllServices.ServiceAbstracts;

namespace University.Application.AllServices
{
    public class DiServices : IServices
    {
        public DiServices(ICourseServices courseServices,
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
