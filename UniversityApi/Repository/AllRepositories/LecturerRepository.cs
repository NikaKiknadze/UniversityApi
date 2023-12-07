using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;
using UniversityApi.Repository.RepositoryAbstracts;

namespace UniversityApi.Repository.Repositoryes
{
    public class LecturerRepository : ILecturerRepository
    {
        private readonly UniversistyContext _context;
        public LecturerRepository(UniversistyContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
           await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IQueryable<Lecturer>> GetLecturersAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => _context.Lecturers.AsQueryable(), cancellationToken);
        }

        public async Task<IQueryable<Lecturer>> GetLecturersWithRelatedDataAsync(CancellationToken cancellationToken)
        {
            var lecturer = await _context.Lecturers
                            .Include(l => l.UsersLecturers)
                                .ThenInclude(ul => ul.User)
                            .Include(l => l.CoursesLecturers)
                                .ThenInclude(cl => cl.Course)
                            .ToListAsync(cancellationToken);
            return lecturer.AsQueryable();
        }

        public async Task<Lecturer> CreateLecturerAsync(Lecturer lecturer, CancellationToken cancellationToken)
        {
            await _context.Lecturers.AddAsync(lecturer, cancellationToken);
            return lecturer;
        }

        public async Task<bool> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken)
        {
            var lecturer = await _context.Lecturers.FirstOrDefaultAsync(l => l.Id == lecturerId, cancellationToken);
            if (lecturer == null)
            {
                return false;
            }
            _context.Lecturers.Remove(lecturer);
            return true;
        }

        public async Task<bool> DeleteUsersLecturersAsync(int lecturerId, CancellationToken cancellationToken)
        {
            var usersLecturers = await _context.UsersLecturersJoin
                                         .Where(l => l.LecturerId == lecturerId)
                                         .ToListAsync(cancellationToken);
            if (usersLecturers == null)
            {
                return false;
            }
            _context.UsersLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }

        public async Task<bool> DeleteCoursesLecturersAsync(int lecturerId, CancellationToken cancellationToken)
        {
            var usersLecturers = await _context.CoursesLecturersJoin
                                         .Where(l => l.LectureId == lecturerId)
                                         .ToListAsync(cancellationToken);
            if (usersLecturers == null)
            {
                return false;
            }
            _context.CoursesLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }

        public async Task<bool> UpdateLecturerAsync(Lecturer updatedLecturer, CancellationToken cancellationToken)
        {
            var existingLecturer = await _context.Lecturers.FirstOrDefaultAsync(l => l.Id == updatedLecturer.Id, cancellationToken);
            if (existingLecturer == null)
            {
                return false;
            }

            existingLecturer.Name = updatedLecturer.Name;
            existingLecturer.SurName = updatedLecturer.SurName;
            existingLecturer.Age = updatedLecturer.Age;
            return true;
        }
    }
}
