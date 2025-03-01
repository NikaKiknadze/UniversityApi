using Microsoft.EntityFrameworkCore;
using University.Data.Data.Entities;
using University.Data.Data.Repository.RepositoryAbstracts;

namespace University.Data.Data.Repository.AllRepositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly UniversistyContext _context;
        public FacultyRepository(UniversistyContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IQueryable<Faculty>> GetFacultiesAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => _context.Faculty.AsQueryable(), cancellationToken);
        }

        public async Task<IQueryable<Faculty>> GetFacultiesWithRelatedDataAsync(CancellationToken cancellationToken)
        {
            var faculty = await _context.Faculty.AsQueryable()
                               .Include(f => f.Users)
                                    .ThenInclude(u => u.UsersCourses)
                                    .ThenInclude(uc => uc.Course)
                               .Include(f => f.Users)
                                    .ThenInclude(u => u.UsersLecturers)
                                    .ThenInclude(ul => ul.Lecturer)
                                .Include(f => f.Courses)
                                .ToListAsync(cancellationToken);
            return faculty.AsQueryable();
        }

        public async Task<Faculty> CreateFacultyAsync(Faculty faculty, CancellationToken cancellationToken)
        {
            await _context.Faculty.AddAsync(faculty, cancellationToken);
            return faculty;
        }

        public async Task<bool> UpdateFacultyAsync(Faculty updatedFaculty, CancellationToken cancellationToken)
        {
            var existingFaculty = await _context.Faculty.FirstOrDefaultAsync(f => f.Id == updatedFaculty.Id, cancellationToken);

            if (existingFaculty == null)
            {
                return false;
            }

            existingFaculty.FacultyName = updatedFaculty.FacultyName;

            return true;
        }

        public async Task<bool> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken)
        {
            var faculty = await _context.Faculty.FirstOrDefaultAsync(f => f.Id == facultyId, cancellationToken);

            if (faculty == null)
            {
                return false;
            }
            var users = await _context.Users.Where(f => f.FacultyId == facultyId).ToListAsync(cancellationToken);
            foreach (var user in users)
            {
                user.FacultyId = null;
            }

            var courses = await _context.Courses.Where(f => f.FacultyId == facultyId).ToListAsync(cancellationToken);
            foreach (var course in courses)
            {
                course.FacultyId = null;
            }

            _context.Faculty.Remove(faculty);
            return true;
        }

    }
}
