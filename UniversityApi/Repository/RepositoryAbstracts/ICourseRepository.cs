using UniversityApi.Entities;

namespace UniversityApi.Repository.RepositoryAbstracts
{
    public interface ICourseRepository
    {
        public Task SaveChangesAsync();
        public Task<Course> GetCourseByIdAsync(int courseId);
        public Task<IQueryable<Course>> GetCoursesWithRelatedDataAsync();
        public Task<IQueryable<Course>> GetCoursesAsync();
        public Task<Course> CreateCourseAsync(Course course);
        public Task<bool> UpdateCourseAsync(Course updatedCourse);
        public Task<bool> DeleteCourseAsync(int courseId);
        public Task<bool> DeleteUsersCoursesAsync(int courseId);
        public Task<bool> DeleteCourseLecturersAsync(int courseId);
    }
}
