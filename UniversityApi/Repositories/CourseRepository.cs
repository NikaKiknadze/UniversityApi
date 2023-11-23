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

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses
                .Include(c => c.UsersCourses)
                                .ThenInclude(uc => uc.User)
                           .Include(c => c.CoursesLecturers)
                                .ThenInclude(cl => cl.Lecturer)
                           .Include(c => c.Faculty)
                .FirstOrDefault(c => c.Id == courseId);
        }

        public List<Course> GetCoursesWithRelatedData()
        {
            return _context.Courses
                           .Include(c => c.UsersCourses)
                                .ThenInclude(uc => uc.User)
                           .Include(c => c.CoursesLecturers)
                                .ThenInclude(cl => cl.Lecturer)
                           .Include(c => c.Faculty)
                           .ToList();
        }

        public IQueryable<Course> GetCourses()
        {
            return _context.Courses.AsQueryable();
        }

        public Course CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            return course;
        }

        public bool UpdateCourse(Course updatedCourse)
        {
            var existingCourse = _context.Courses.FirstOrDefault(c => c.Id == updatedCourse.Id);

            if (existingCourse == null)
            {
                return false;
            }

            existingCourse.CourseName = updatedCourse.CourseName;
            existingCourse.FacultyId = updatedCourse.FacultyId;
            return true;
        }

        public bool DeleteCourse(int courseId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);
            return true;
        }

        public bool DeleteUsersCourses(int courseId)
        {
            var usersCourses = _context.UsersCoursesJoin
                                       .Where(c => c.CourseId == courseId);
            if (usersCourses == null)
            {
                return false;
            }
            _context.UsersCoursesJoin.RemoveRange(usersCourses);
            return true;
        }

        public bool DeleteCourseLecturers(int courseId)
        {
            var courseLecturers = _context.CoursesLecturersJoin
                                         .Where(c => c.CourseId == courseId);
            if (courseLecturers == null)
            {
                return false;
            }
            _context.CoursesLecturersJoin.RemoveRange(courseLecturers);
            return true;
        }
    }
}
