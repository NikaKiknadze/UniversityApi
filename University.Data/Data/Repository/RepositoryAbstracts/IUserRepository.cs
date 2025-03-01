using University.Data.Data.Entities;

namespace University.Data.Data.Repository.RepositoryAbstracts
{
    public interface IUserRepository
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task<IQueryable<User>> GetUsersWithRelatedDataAsync(CancellationToken cancellationToken);
        Task<IQueryable<User>> GetUsersAsync(CancellationToken cancellationToken);
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
        Task<bool> UpdateUserAsync(User updatedUser, CancellationToken cancellationToken);
        Task<bool> DeleteUserAsync(int userId, CancellationToken cancellationToken);
        Task<bool> DeleteUsersCoursesAsync(int userId, CancellationToken cancellationToken);
        Task<bool> DeleteUsersLecturers(int userId, CancellationToken cancellationToken);
    }
}
