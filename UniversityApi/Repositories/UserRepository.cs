using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;

namespace UniversityApi.Repositories
{
    public class UserRepository
    {
        private readonly UniversistyContext _context;
        public UserRepository(UniversistyContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public User GetUserById(int userId)
        {
            return _context.Users
                .Include(u => u.Faculty)
                .Include(u => u.UsersCourses)
                .Include(u => u.UsersLecturers)
                .FirstOrDefault(u => u.Id == userId);
        }

        public List<User> GetUsersWithRelatedData()
        {
            return _context.Users
                           .Include(u => u.UsersCourses)
                           .Include(u => u.UsersLecturers)
                           .ToList();
        }

        public IQueryable<User> GetUsers()
        {
            return _context.Users.AsQueryable();
        }
        
        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            return user;
        }
        
        public bool UpdateUser(User updatedUser)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == updatedUser.Id);

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

        public bool DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            return true;
        }

        public bool DeleteUsersCourses(int userId)
        {
            var usersCourses = _context.UsersCoursesJoin
                                       .Where(u => u.UserId == userId);
            if(usersCourses == null)
            {
                return false;
            }
            _context.UsersCoursesJoin.RemoveRange(usersCourses);
            return true;
        }

        public bool DeleteUsersLecturers(int userId)
        {
            var usersLecturers = _context.UsersLecturersJoin
                                         .Where(u => u.UserId == userId);
            if(usersLecturers == null)
            {
                return false;
            }
            _context.UsersLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }
    }
}
