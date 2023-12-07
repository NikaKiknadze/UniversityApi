using UniversityApi.Entities;

namespace UniversityApi.Repository.RepositoryAbstracts
{
    public interface IFacultyRepository
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<IQueryable<Faculty>> GetFacultiesAsync(CancellationToken cancellationToken);
        Task<IQueryable<Faculty>> GetFacultiesWithRelatedDataAsync(CancellationToken cancellationToken);
        Task<Faculty> CreateFacultyAsync(Faculty faculty, CancellationToken cancellationToken);
        Task<bool> UpdateFacultyAsync(Faculty updatedFaculty, CancellationToken cancellationToken);
        Task<bool> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken);
    }
}
