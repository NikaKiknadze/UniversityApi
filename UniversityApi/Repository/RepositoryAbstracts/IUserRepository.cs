using UniversityApi.Entities;

namespace UniversityApi.Repository.RepositoryAbstracts
{
    public interface IUserRepository
    {
        public Task SaveChangesAsync();
        public Task<User> GetUserByIdAsync(int userId);
        public Task<IQueryable<User>> GetUsersWithRelatedDataAsync();
        public Task<IQueryable<User>> GetUsersAsync();
        public Task<User> CreateUserAsync(User user);
        public Task<bool> UpdateUserAsync(User updatedUser);
        public Task<bool> DeleteUserAsync(int userId);
        public Task<bool> DeleteUsersCoursesAsync(int userId);
        public Task<bool> DeleteUsersLecturers(int userId);
    }
}
