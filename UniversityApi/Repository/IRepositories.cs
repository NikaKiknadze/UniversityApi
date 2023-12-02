using UniversityApi.Repository.RepositoryAbstracts;

namespace UniversityApi.Repository
{
    public interface IRepositories
    {
        ICourseRepository CourseRepository { get; }
        IFacultyRepository FacultyRepository { get; }
        ILecturerRepository LecturerRepository { get; }
        IUserRepository UserRepository { get; }
    }
}
