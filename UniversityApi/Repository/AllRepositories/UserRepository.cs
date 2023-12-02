using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;
using UniversityApi.Repository.RepositoryAbstracts;

namespace UniversityApi.Repository.Repositoryes
{
    public class UserRepository : IUserRepository
    {
        private readonly UniversistyContext _context;
        public UserRepository(UniversistyContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Faculty)
                .Include(u => u.UsersCourses)
                .Include(u => u.UsersLecturers)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IQueryable<User>> GetUsersWithRelatedDataAsync()
        {
            var user = await _context.Users
                           .Include(u => u.Faculty)
                           .Include(uc => uc.UsersCourses)
                                .ThenInclude(u => u.Course)
                           .Include(ul => ul.UsersLecturers)
                                .ThenInclude(u => u.Lecturer)
                           .ToListAsync();
            return user.AsQueryable();
        }

        public async Task<IQueryable<User>> GetUsersAsync()
        {
            return await Task.Run(() => _context.Users.AsQueryable());
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

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

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            return true;
        }

        public async Task<bool> DeleteUsersCoursesAsync(int userId)
        {
            var usersCourses = await _context.UsersCoursesJoin
                                       .Where(u => u.UserId == userId)
                                       .ToListAsync();
            if (usersCourses == null)
            {
                return false;
            }
            _context.UsersCoursesJoin.RemoveRange(usersCourses);
            return true;
        }

        public async Task<bool> DeleteUsersLecturers(int userId)
        {
            var usersLecturers = await _context.UsersLecturersJoin
                                         .Where(u => u.UserId == userId)
                                         .ToListAsync();
            if (usersLecturers == null)
            {
                return false;
            }
            _context.UsersLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }
    }
}
