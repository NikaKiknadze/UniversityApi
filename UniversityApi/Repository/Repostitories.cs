using UniversityApi.Repository.RepositoryAbstracts;

namespace UniversityApi.Repository
{
    public class Repostitories : IRepositories
    {
        public Repostitories(ICourseRepository courseRepository, 
                             IFacultyRepository facultyRepository, 
                             ILecturerRepository lecturerRepository, 
                             IUserRepository userRepository)
        {
            CourseRepository = courseRepository;
            FacultyRepository = facultyRepository;
            LecturerRepository = lecturerRepository;
            UserRepository = userRepository;
        }
        public ICourseRepository CourseRepository { get; } 
        public IFacultyRepository FacultyRepository { get; }
        public ILecturerRepository LecturerRepository { get; }
        public IUserRepository UserRepository { get; }

    }
}
