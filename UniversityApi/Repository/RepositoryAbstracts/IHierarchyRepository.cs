using UniversityApi.Entities;

namespace UniversityApi.Repository.RepositoryAbstracts
{
    public interface IHierarchyRepository
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<IQueryable<Hierarchy>> GetDataAsync(CancellationToken cancellationToken);
        Task<Hierarchy> CreateObjectAsync(Hierarchy hierarchy, CancellationToken cancellationToken);
        Task<bool> UpdateObjectAsync(Hierarchy updatedHierarchy, CancellationToken cancellationToken);
        Task<bool> DeleteHierarchyObjectAsync(int hierarchyId, CancellationToken cancellationToken);
    }
}
