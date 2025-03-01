using University.Data.Data.Repository.RepositoryAbstracts;

namespace University.Data.Data.Repository
{
    public class Repostitories : IRepositories
    {
        public Repostitories(ICourseRepository courseRepository, 
                             IFacultyRepository facultyRepository, 
                             ILecturerRepository lecturerRepository, 
                             IUserRepository userRepository,
                             IHierarchyRepository hierarchyRepository)
        {
            CourseRepository = courseRepository;
            FacultyRepository = facultyRepository;
            LecturerRepository = lecturerRepository;
            UserRepository = userRepository;
            HierarchyRepository = hierarchyRepository;
        }
        public ICourseRepository CourseRepository { get; } 
        public IFacultyRepository FacultyRepository { get; }
        public ILecturerRepository LecturerRepository { get; }
        public IUserRepository UserRepository { get; }
        public IHierarchyRepository HierarchyRepository { get; }
    }
}
