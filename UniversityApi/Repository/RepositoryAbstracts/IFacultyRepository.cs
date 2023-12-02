using UniversityApi.Entities;

namespace UniversityApi.Repository.RepositoryAbstracts
{
    public interface IFacultyRepository
    {
        public Task SaveChangesAsync();
        public Task<Faculty> GetFacultieByIdAsync(int userId);
        public Task<IQueryable<Faculty>> GetFacultiesAsync();
        public Task<IQueryable<Faculty>> GetFacultiesWithRelatedDataAsync();
        public Task<Faculty> CreateFacultyAsync(Faculty faculty);
        public Task<bool> UpdateFacultyAsync(Faculty updatedFaculty);
        public Task<bool> DeleteFacultyAsync(int facultyId);
    }
}
