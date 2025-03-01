using Microsoft.EntityFrameworkCore;
using University.Data.Data.Entities;
using University.Data.Data.Repository.RepositoryAbstracts;

namespace University.Data.Data.Repository.AllRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UniversistyContext _context;
        public UserRepository(UniversistyContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IQueryable<User>> GetUsersWithRelatedDataAsync(CancellationToken cancellationToken)
        {
            var user = await _context.Users
                           .Include(u => u.Faculty)
                           .Include(uc => uc.UsersCourses)
                                .ThenInclude(u => u.Course)
                           .Include(ul => ul.UsersLecturers)
                                .ThenInclude(u => u.Lecturer)
                           .ToListAsync(cancellationToken);
            return user.AsQueryable();
        }

        public async Task<IQueryable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => _context.Users.AsQueryable(), cancellationToken);
        }

        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            return user;
        }

        public async Task<bool> UpdateUserAsync(User updatedUser, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id, cancellationToken);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.Name = updatedUser.Name;
            existingUser.SurName = updatedUser.SurName;
            existingUser.Age = updatedUser.Age;
            existingUser.FacultyId = updatedUser.FacultyId;

            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            return true;
        }

        public async Task<bool> DeleteUsersCoursesAsync(int userId, CancellationToken cancellationToken)
        {
            var usersCourses = await _context.UsersCoursesJoin
                                       .Where(u => u.UserId == userId)
                                       .ToListAsync(cancellationToken);
            if (usersCourses == null)
            {
                return false;
            }
            _context.UsersCoursesJoin.RemoveRange(usersCourses);
            return true;
        }

        public async Task<bool> DeleteUsersLecturers(int userId, CancellationToken cancellationToken)
        {
            var usersLecturers = await _context.UsersLecturersJoin
                                         .Where(u => u.UserId == userId)
                                         .ToListAsync(cancellationToken);
            if (usersLecturers == null)
            {
                return false;
            }
            _context.UsersLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }
    }
}
