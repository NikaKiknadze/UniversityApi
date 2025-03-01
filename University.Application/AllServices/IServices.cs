using University.Application.AllServices.ServiceAbstracts;

namespace University.Application.AllServices
{
    public interface IServices
    {
        ICourseServices CourseServices { get; }
        IFacultyServices FacultyServices { get; }
        ILecturerServices LecturerServices { get; }
        IUserServices UserServices { get; }
    }
}
