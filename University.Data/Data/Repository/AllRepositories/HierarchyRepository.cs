using Microsoft.EntityFrameworkCore;
using University.Data.Data.Entities;
using University.Data.Data.Repository.RepositoryAbstracts;

namespace University.Data.Data.Repository.AllRepositories
{
    public class HierarchyRepository : IHierarchyRepository
    {
        private readonly UniversistyContext _context;

        public HierarchyRepository(UniversistyContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Hierarchy> CreateObjectAsync(Hierarchy hierarchy, CancellationToken cancellationToken)
        {
            await _context.Hierarchy.AddAsync(hierarchy, cancellationToken);
            return hierarchy;
        }

        public async Task<IQueryable<Hierarchy>> GetDataAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => _context.Hierarchy.AsQueryable(), cancellationToken);
        }

        public async Task<bool> UpdateObjectAsync(Hierarchy updatedHierarchy, CancellationToken cancellationToken)
        {
            var existingHierarchyObject = await _context.Hierarchy.FirstOrDefaultAsync(h => h.Id == updatedHierarchy.Id, cancellationToken);
            if(existingHierarchyObject == null)
            {
                return false;
            }
            existingHierarchyObject.ParentId = updatedHierarchy.ParentId;
            existingHierarchyObject.SortIndex = updatedHierarchy.SortIndex;
            return true;
        }

        public async Task<bool> DeleteHierarchyObjectAsync(int hierarchyId, CancellationToken cancellationToken)
        {
            var hierarchyObject = await _context.Hierarchy.FirstOrDefaultAsync(h => h.Id == hierarchyId, cancellationToken);
            if(hierarchyObject == null)
            {
                return false;
            }

            _context.Hierarchy.Remove(hierarchyObject);
            return true;
        }
    }
}
