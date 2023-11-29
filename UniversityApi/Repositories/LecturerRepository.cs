using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;

namespace UniversityApi.Repositories
{
    public class LecturerRepository
    {
        private readonly UniversistyContext _context;
        public LecturerRepository(UniversistyContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Lecturer>> GetLecturersAsync()
        {
            return await Task.Run(() => _context.Lecturers.AsQueryable());
        }

        public async Task<IQueryable<Lecturer>> GetLecturersWithRelatedDataAsync()
        {
            var lecturer = await _context.Lecturers
                            .Include(l => l.UsersLecturers)
                                .ThenInclude(ul => ul.User)
                            .Include(l => l.CoursesLecturers)
                                .ThenInclude(cl => cl.Course)
                            .ToListAsync();
            return lecturer.AsQueryable();
        }

        public async Task<Lecturer> CreateLecturerAsync(Lecturer lecturer)
        {
            await _context.Lecturers.AddAsync(lecturer);
            return lecturer;
        }

        public async Task<bool> DeleteLecturerAsync(int lecturerId)
        {
            var lecturer = await _context.Lecturers.FirstOrDefaultAsync(l => l.Id == lecturerId);
            if (lecturer == null)
            {
                return false;
            }
            _context.Lecturers.Remove(lecturer);
            return true;
        }

        public async Task<bool> DeleteUsersLecturersAsync(int lecturerId)
        {
            var usersLecturers = await _context.UsersLecturersJoin
                                         .Where(l => l.LecturerId == lecturerId)
                                         .ToListAsync();
            if (usersLecturers == null)
            {
                return false;
            }
            _context.UsersLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }

        public async Task<bool> DeleteCoursesLecturersAsync(int lecturerId)
        {
            var usersLecturers = await _context.CoursesLecturersJoin
                                         .Where(l => l.LectureId == lecturerId)
                                         .ToListAsync();
            if (usersLecturers == null)
            {
                return false;
            }
            _context.CoursesLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }

        public async Task<bool> UpdateLecturerAsync(Lecturer updatedLecturer)
        {
            var existingLecturer = await _context.Lecturers.FirstOrDefaultAsync(l => l.Id == updatedLecturer.Id);
            if (existingLecturer == null)
            {
                return false;
            }

            existingLecturer.Name = updatedLecturer.Name;
            existingLecturer.SurName = updatedLecturer.SurName;
            existingLecturer.Age = (int)updatedLecturer.Age;
            return true;
        }
    }
}
