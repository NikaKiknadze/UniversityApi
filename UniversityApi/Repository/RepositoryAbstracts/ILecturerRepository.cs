using UniversityApi.Entities;

namespace UniversityApi.Repository.RepositoryAbstracts
{
    public interface ILecturerRepository
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<IQueryable<Lecturer>> GetLecturersAsync(CancellationToken cancellationToken);
        Task<IQueryable<Lecturer>> GetLecturersWithRelatedDataAsync(CancellationToken cancellationToken);
        Task<Lecturer> CreateLecturerAsync(Lecturer lecturer, CancellationToken cancellationToken);
        Task<bool> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken);
        Task<bool> DeleteUsersLecturersAsync(int lecturerId, CancellationToken cancellationToken);
        Task<bool> DeleteCoursesLecturersAsync(int lecturerId, CancellationToken cancellationToken);
        Task<bool> UpdateLecturerAsync(Lecturer updatedLecturer, CancellationToken cancellationToken);
    }
}
