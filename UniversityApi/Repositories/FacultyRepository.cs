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
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Faculty> GetFacultieByIdAsync(int userId)
        {
            return await _context.Faculty
                                 .Include(f => f.Users)
                                 .Include(f => f.Courses)
                                 .FirstOrDefaultAsync(f => f.Id == userId);

        }

        public async Task<IQueryable<Faculty>> GetFacultiesAsync()
        {
            return await Task.Run(() => _context.Faculty.AsQueryable());
        }

        public async Task<IQueryable<Faculty>> GetFacultiesWithRelatedDataAsync()
        {
            var faculty = await _context.Faculty.AsQueryable()
                               .Include(f => f.Users)
                                    .ThenInclude(u => u.UsersCourses)
                                    .ThenInclude(uc => uc.Course)
                               .Include(f => f.Users)
                                    .ThenInclude(u => u.UsersLecturers)
                                    .ThenInclude(ul => ul.Lecturer)
                                .Include(f => f.Courses)
                                .ToListAsync();
            return faculty.AsQueryable();
        }

        public async Task<Faculty> CreateFacultyAsync(Faculty faculty)
        {
            await _context.Faculty.AddAsync(faculty);
            return faculty;
        }

        public async Task<bool> UpdateFacultyAsync(Faculty updatedFaculty)
        {
            var existingFaculty = await _context.Faculty.FirstOrDefaultAsync(f => f.Id == updatedFaculty.Id);

            if (existingFaculty == null)
            {
                return false;
            }

            existingFaculty.FacultyName = updatedFaculty.FacultyName;

            return true;
        }

        public async Task<bool> DeleteFacultyAsync(int facultyId)
        {
            var faculty = await _context.Faculty.FirstOrDefaultAsync(f => f.Id == facultyId);

            if (faculty == null)
            {
                return false;
            }
            var users = await _context.Users.Where(f => f.FacultyId == facultyId).ToListAsync();
            foreach (var user in users)
            {
                user.FacultyId = null;
            }

            var courses = await _context.Courses.Where(f => f.FacultyId == facultyId).ToListAsync();
            foreach (var course in courses)
            {
                course.FacultyId = null;
            }

            _context.Faculty.Remove(faculty);
            return true;
        }

    }
}
