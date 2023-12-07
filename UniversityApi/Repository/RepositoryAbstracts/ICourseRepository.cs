using UniversityApi.Entities;

namespace UniversityApi.Repository.RepositoryAbstracts
{
    public interface ICourseRepository
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<Course> GetCourseByIdAsync(int courseId, CancellationToken cancellationToken);
        Task<IQueryable<Course>> GetCoursesWithRelatedDataAsync(CancellationToken cancellationToken);
        Task<IQueryable<Course>> GetCoursesAsync(CancellationToken cancellationToken);
        Task<Course> CreateCourseAsync(Course course, CancellationToken cancellationToken);
        Task<bool> UpdateCourseAsync(Course updatedCourse, CancellationToken cancellationToken);
        Task<bool> DeleteCourseAsync(int courseId, CancellationToken cancellationToken);
        Task<bool> DeleteUsersCoursesAsync(int courseId, CancellationToken cancellationToken);
        Task<bool> DeleteCourseLecturersAsync(int courseId, CancellationToken cancellationToken);
    }
}
