using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;
using UniversityApi.Repository.RepositoryAbstracts;

namespace UniversityApi.Repository.Repositoryes
{
    public class CourseRepository : ICourseRepository
    {
        private readonly UniversistyContext _context;
        public CourseRepository(UniversistyContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Course> GetCourseByIdAsync(int courseId, CancellationToken cancellationToken)
        {
            return await _context.Courses
                .Include(c => c.UsersCourses)
                    .ThenInclude(uc => uc.User)
                .Include(c => c.CoursesLecturers)
                    .ThenInclude(cl => cl.Lecturer)
                .Include(c => c.Faculty)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<IQueryable<Course>> GetCoursesWithRelatedDataAsync(CancellationToken cancellationToken)
        {
            var course = await _context.Courses
                           .Include(c => c.UsersCourses)
                                .ThenInclude(uc => uc.User)
                           .Include(c => c.CoursesLecturers)
                                .ThenInclude(cl => cl.Lecturer)
                           .Include(c => c.Faculty)
                           .ToListAsync(cancellationToken);
            return course.AsQueryable();
        }

        public async Task<IQueryable<Course>> GetCoursesAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => _context.Courses.AsQueryable(), cancellationToken);
        }

        public async Task<Course> CreateCourseAsync(Course course, CancellationToken cancellationToken)
        {
            await _context.Courses.AddAsync(course, cancellationToken);
            return course;
        }

        public async Task<bool> UpdateCourseAsync(Course updatedCourse, CancellationToken cancellationToken)
        {
            var existingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.Id == updatedCourse.Id, cancellationToken);

            if (existingCourse == null)
            {
                return false;
            }

            existingCourse.CourseName = updatedCourse.CourseName;
            existingCourse.FacultyId = updatedCourse.FacultyId;
            return true;
        }

        public async Task<bool> DeleteCourseAsync(int courseId, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId, cancellationToken);

            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);
            return true;
        }

        public async Task<bool> DeleteUsersCoursesAsync(int courseId, CancellationToken cancellationToken)
        {
            var usersCourses = await _context.UsersCoursesJoin
                                       .Where(c => c.CourseId == courseId)
                                       .ToListAsync(cancellationToken);
            if (usersCourses == null)
            {
                return false;
            }
            _context.UsersCoursesJoin.RemoveRange(usersCourses);
            return true;
        }

        public async Task<bool> DeleteCourseLecturersAsync(int courseId, CancellationToken cancellationToken)
        {
            var courseLecturers = await _context.CoursesLecturersJoin
                                         .Where(c => c.CourseId == courseId)
                                         .ToListAsync(cancellationToken);
            if (courseLecturers == null)
            {
                return false;
            }
            _context.CoursesLecturersJoin.RemoveRange(courseLecturers);
            return true;
        }
    }
}
