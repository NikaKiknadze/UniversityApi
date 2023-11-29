using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Entities;

namespace UniversityApi.Repositories
{
    public class CourseRepository
    {
        private readonly UniversistyContext _context;
        public CourseRepository(UniversistyContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.UsersCourses)
                    .ThenInclude(uc => uc.User)
                .Include(c => c.CoursesLecturers)
                    .ThenInclude(cl => cl.Lecturer)
                .Include(c => c.Faculty)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<IQueryable<Course>> GetCoursesWithRelatedDataAsync()
        {
            var course =  await _context.Courses
                           .Include(c => c.UsersCourses)
                                .ThenInclude(uc => uc.User)
                           .Include(c => c.CoursesLecturers)
                                .ThenInclude(cl => cl.Lecturer)
                           .Include(c => c.Faculty)
                           .ToListAsync();
            return course.AsQueryable();
        }

        public async Task<IQueryable<Course>> GetCoursesAsync()
        {
            return await Task.Run(() => _context.Courses.AsQueryable());
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            return course;
        }

        public async Task<bool> UpdateCourseAsync(Course updatedCourse)
        {
            var existingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.Id == updatedCourse.Id);

            if (existingCourse == null)
            {
                return false;
            }

            existingCourse.CourseName = updatedCourse.CourseName;
            existingCourse.FacultyId = updatedCourse.FacultyId;
            return true;
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);
            return true;
        }

        public async Task<bool> DeleteUsersCoursesAsync(int courseId)
        {
            var usersCourses = await _context.UsersCoursesJoin
                                       .Where(c => c.CourseId == courseId)
                                       .ToListAsync();
            if (usersCourses == null)
            {
                return false;
            }
            _context.UsersCoursesJoin.RemoveRange(usersCourses);
            return true;
        }

        public async Task<bool> DeleteCourseLecturersAsync(int courseId)
        {
            var courseLecturers = await _context.CoursesLecturersJoin
                                         .Where(c => c.CourseId == courseId)
                                         .ToListAsync();
            if (courseLecturers == null)
            {
                return false;
            }
            _context.CoursesLecturersJoin.RemoveRange(courseLecturers);
            return true;
        }
    }
}
