using UniversityApi.Service.ServiceAbstracts;

namespace UniversityApi.Service
{
    public interface IServices
    {
        ICourseServices CourseServices { get; }
        IFacultyServices FacultyServices { get; }
        ILecturerServices LecturerServices { get; }
        IUserServices UserServices { get; }
    }
}
