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

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IQueryable<Lecturer> GetLecturers()
        {
            return _context.Lecturers.AsQueryable();
        }

        public List<Lecturer> GetLecturersWithRelatedData()
        {
            return _context.Lecturers
                            .Include(l => l.UsersLecturers)
                                .ThenInclude(ul => ul.User)
                            .Include(l => l.CoursesLecturers)
                                .ThenInclude(cl => cl.Course)
                            .ToList();
        }

        public Lecturer CreateLecturer(Lecturer lecturer)
        {
            _context.Lecturers.Add(lecturer);
            return lecturer;
        }

        public bool DeleteLecturer(int lecturerId)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(l => l.Id == lecturerId);
            if (lecturer == null)
            {
                return false;
            }
            _context.Lecturers.Remove(lecturer);
            return true;
        }

        public bool DeleteUsersLecturers(int lecturerId)
        {
            var usersLecturers = _context.UsersLecturersJoin
                                         .Where(l => l.LecturerId == lecturerId);
            if (usersLecturers == null)
            {
                return false;
            }
            _context.UsersLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }

        public bool DeleteCoursesLecturers(int lecturerId)
        {
            var usersLecturers = _context.CoursesLecturersJoin
                                         .Where(l => l.LectureId == lecturerId);
            if (usersLecturers == null)
            {
                return false;
            }
            _context.CoursesLecturersJoin.RemoveRange(usersLecturers);
            return true;
        }

        public bool UpdateLecturer(Lecturer updatedLecturer)
        {
            var existingLecturer = _context.Lecturers.FirstOrDefault(l => l.Id == updatedLecturer.Id);
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
