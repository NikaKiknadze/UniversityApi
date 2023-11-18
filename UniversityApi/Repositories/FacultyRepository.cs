using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;

namespace UniversityApi.Repositories
{
    public class FacultyRepository
    {
        private readonly UniversistyContext _context;
        public FacultyRepository(UniversistyContext context)
        {
            _context = context;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Faculty GetFacultieById(int userId)
        {
            return _context.Faculty
                                 .Include(f => f.Users)
                                 .Include(f => f.Courses)
                                 .FirstOrDefault(f => f.Id == userId);

        }

        public IQueryable<Faculty> GetFaculties()
        {
            return _context.Faculty.AsQueryable();
        }

        public Faculty CreateFaculty(Faculty faculty)
        {
            _context.Faculty.Add(faculty);
            return faculty;
        }

        public bool UpdateFaculty(Faculty updatedFaculty)
        {
            var existingFaculty = _context.Faculty.FirstOrDefault(f => f.Id == updatedFaculty.Id);

            if (existingFaculty == null)
            {
                return false;
            }

            existingFaculty.FacultyName = updatedFaculty.FacultyName;

            return true;
        }

        public bool DeleteFaculty(int facultyId)
        {
            var faculty = _context.Faculty.FirstOrDefault(f => f.Id == facultyId);

            if (faculty == null)
            {
                return false;
            }
            var users = _context.Users.Where(f => f.FacultyId == facultyId).ToList(); 
            foreach(var user in users)
            {
                user.FacultyId = null;
            }
            _context.Faculty.Remove(faculty);
            return true;
        }

    }
}
