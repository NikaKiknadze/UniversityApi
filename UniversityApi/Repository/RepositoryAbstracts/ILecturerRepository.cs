using UniversityApi.Entities;

namespace UniversityApi.Repository.RepositoryAbstracts
{
    public interface ILecturerRepository
    {
        public Task SaveChangesAsync();
        public Task<IQueryable<Lecturer>> GetLecturersAsync();
        public Task<IQueryable<Lecturer>> GetLecturersWithRelatedDataAsync();
        public Task<Lecturer> CreateLecturerAsync(Lecturer lecturer);
        public Task<bool> DeleteLecturerAsync(int lecturerId);
        public Task<bool> DeleteUsersLecturersAsync(int lecturerId);
        public Task<bool> DeleteCoursesLecturersAsync(int lecturerId);
        public Task<bool> UpdateLecturerAsync(Lecturer updatedLecturer);
    }
}
