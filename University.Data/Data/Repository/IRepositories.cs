using University.Data.Data.Repository.RepositoryAbstracts;

namespace University.Data.Data.Repository
{
    public interface IRepositories
    {
        ICourseRepository CourseRepository { get; }
        IFacultyRepository FacultyRepository { get; }
        ILecturerRepository LecturerRepository { get; }
        IUserRepository UserRepository { get; }
        IHierarchyRepository HierarchyRepository { get; }
    }
}
