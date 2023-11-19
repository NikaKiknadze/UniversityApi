using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repositories;

namespace UniversityApi.Services
{
    public class CourseServices
    {
        public readonly UniversistyContext _context;
        public readonly CourseRepository _courseRepository;

        public CourseServices(UniversistyContext context, CourseRepository repository)
        {
            _context = context;
            _courseRepository = repository;
        }

        public List<CourseGetDto> GetCourses()
        {
            var courses = _courseRepository.GetCoursesWithRelatedData();

            var courseDtos = courses.Select(course => new CourseGetDto
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Faculty = new FacultyGetDto
                {
                    Id = (int)course.Faculty.Id,
                    FacultyName = (string)course.Faculty.FacultyName
                },
                LecturerIds = course.CoursesLecturers.Select(c => c.LectureId).ToList() ?? new List<int>(),
                UserIds = course.UsersCourses.Select(c => c.UserId).ToList() ?? new List<int>()
            }).ToList();

            return courseDtos;
        }

        public CourseGetDto CreateCourse(CoursePostDto input)
        {
            var course = new Course
            {
                CourseName = input.CourseName,
                UsersCourses = new List<UsersCoursesJoin>(),
                CoursesLecturers = new List<CoursesLecturersJoin>
            };

            _courseRepository.CreateCourse(course);
            _courseRepository.SaveChanges();

            if (!input.UserIds.IsNullOrEmpty())
            {
                course.UsersCourses = new List<UsersCoursesJoin>();

                foreach(var userId in input.UserIds)
                {
                    course.UsersCourses.Add(new UsersCoursesJoin()
                    {
                        UserId = userId,
                        CourseId = course.Id
                    });
                }
            }

            if(!input.LecturerIds.IsNullOrEmpty())
            {
                course.CoursesLecturers = new List<CoursesLecturersJoin>();

                foreach(var lecturerId in input.LecturerIds)
                {
                    course.CoursesLecturers.Add(new CoursesLecturersJoin()
                    {
                        LectureId = lecturerId,
                        CourseId = course.Id
                    });
                }
            }

            return new CourseGetDto
            {
                Id = course.Id,
                CourseName = course.CourseName,
                UserIds = course.UsersCourses.Select(c => c.UserId).ToList(),
                LecturerIds = course.CoursesLecturers.Select(c => c.LectureId).ToList()
            };
        }

        public bool UpdateCourse(CoursePutDto input)
        {
            var course = _courseRepository.GetCourses()
                                          .Include(c => c.UsersCourses)
                                          .Include(c => c.CoursesLecturers)
                                          .Where(c => c.Id == input.Id)
                                          .FirstOrDefault();
            course.Id = input.Id.HasValue ? (int)input.Id : 0;
            course.CourseName = input.CourseName;
            course.FacultyId = input.FacultyId.HasValue ? (int)input.FacultyId : null;

            if (_courseRepository.UpdateCourse(course))
            {
                if (!input.UserIds.IsNullOrEmpty())
                {
                    foreach (var userId in input.UserIds)
                    {
                        course.UsersCourses.Clear();
                        course.UsersCourses.Add(new UsersCoursesJoin()
                        {
                            UserId = userId,
                            CourseId = course.Id
                        });
                    }
                }

                if (!input.LecturerIds.IsNullOrEmpty())
                {
                    foreach(var lecturerId in input.LecturerIds)
                    {
                        course.CoursesLecturers.Clear();
                        course.CoursesLecturers.Add(new CoursesLecturersJoin()
                        {
                            LectureId = lecturerId,
                            CourseId = course.Id
                        });
                    }
                }
                _courseRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteCourse(CourseDeleteDto input)
        {
            if(_courseRepository.DeleteCourse(input.Id) &&
               _courseRepository.DeleteUsersCourses(input.Id) &&
               _courseRepository.DeleteCourseLecturers(input.Id))
            {
                _courseRepository.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
