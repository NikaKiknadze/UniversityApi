using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using University.Application.AllServices.ServiceAbstracts;
using University.Data.ContextMethodsDirectory;
using University.Data.Data;
using University.Data.Data.Entities;
using University.Data.Data.EntityGenericMethods;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.AllServices
{
    public class CourseServices : ICourseServices
    {
        private readonly IUniversityContext _universityContext;

        public CourseServices(IUniversityContext universityContext)
        {
            _universityContext = universityContext;
        }

        public async Task<ApiResponse<GetDtoWithCount<List<CourseGetDto>>>> GetCoursesAsync(CourseGetFilter filter, CancellationToken cancellationToken)
        {
            var courses = await _universityContext.Courses.All.GetCoursesWithRelatedDataAsync(cancellationToken);

            if (courses == null || !courses.Any())
            {
                throw new  NoContentException("No courses found.");
            }
            var filteredCourses = FilterData(filter, courses);

            if(filteredCourses.Count == 0)
            {
                throw new  NotFoundException("Course not found");
            }

            var courseDtos = filteredCourses.Select(course => new CourseGetDto
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Faculty = course.Faculty != null
                              ? new FacultyOnlyDto
                              {
                                  Id = course.Faculty.Id,
                                  FacultyName = course.Faculty.FacultyName
                              }
                              : null,
                Lecturers = course.CoursesLecturers != null
                                   ? course.CoursesLecturers.Where(ul => ul.Lecturer != null).Select(c => new LecturerOnlyDto
                                   {
                                       Id = c.Lecturer.Id,
                                       Name = c.Lecturer.Name,
                                       SurName = c.Lecturer.SurName,
                                       Age = c.Lecturer.Age
                                   }).ToList()
                                   : new List<LecturerOnlyDto>(),
                Users = course.UsersCourses != null
                               ? course.UsersCourses.Where(uc => uc.User != null).Select(c => new UserOnlyDto
                               {
                                   Id = c.User.Id,
                                   Name = c.User.Name,
                                   SurName = c.User.SurName,
                                   Age = c.User.Age
                               }).ToList()
                               : new List<UserOnlyDto>()
            })
                .OrderByDescending(c => c.Id)
                .Skip(filter.Offset ?? 0)
                .Take(filter.Limit ?? 10)
                .ToList();

            return ApiResponse<GetDtoWithCount<List<CourseGetDto>>>.SuccesResult(new GetDtoWithCount<List<CourseGetDto>>
            {
                Data = courseDtos,
                Count = filteredCourses.Count()
            });
        }

        public List<Course> FilterData(CourseGetFilter filter, IQueryable<Course> courses)
        {
            if(filter.Id != null)
            {
                courses = courses.Where(c => c.Id == filter.Id);
            }
            if (!filter.CourseName.IsNullOrEmpty())
            {
                courses = courses.Where(c => c.CourseName.Contains(filter.CourseName));
            }
            if(filter.FacultyId != null)
            {
                courses = courses.Where(c => c.FacultyId == filter.FacultyId);
            }
            if(filter.LecturerIds != null && filter.LecturerIds.Any())
            {
                courses = courses.Where(c => c.CoursesLecturers
                             .Select(c => c.LectureId)
                             .Any(courseId => filter.LecturerIds.Contains(courseId)));
            }
            if(filter.UserIds != null && filter.UserIds.Any())
            {
                courses = courses.Where(c => c.UsersCourses
                                 .Select(c => c.UserId)
                                 .Any(courseId => filter.UserIds.Contains(courseId)));
            }
            return courses.ToList();
        }

        public async Task<ApiResponse<CourseGetDto>> CreateCourseAsync(CoursePostDto input, CancellationToken cancellationToken)
        {
            var course = new Course
            {
                CourseName = input.CourseName,
                FacultyId = input.FacultyId
            };

            if (input.CourseName == null)
            {
                throw new BadRequestException("Course Name is null");
            }

            if (input.UserIds is {Count: > 0})
            {
                course.UsersCourses = new List<UsersCoursesJoin>();

                foreach (var userId in input.UserIds)
                {
                    course.UsersCourses.Add(new UsersCoursesJoin
                    {
                        UserId = userId,
                        CourseId = course.Id
                    });
                }
            }

            if (input.LecturerIds  is {Count: > 0})
            {
                course.CoursesLecturers = new List<CoursesLecturersJoin>();

                foreach (var lecturerId in input.LecturerIds)
                {
                    course.CoursesLecturers.Add(new CoursesLecturersJoin
                    {
                        LectureId = lecturerId,
                        CourseId = course.Id
                    });
                }
            }

            await _universityContext.Courses.AddAsync(course, cancellationToken);
            await _universityContext.CompleteAsync(cancellationToken);

            var courseQueryable = await _universityContext.Courses.GetCoursesAsync(cancellationToken);
            var fetchedCourse = await courseQueryable
                                            .Include(c => c.Faculty)
                                            .Include(c => c.UsersCourses)
                                                    .ThenInclude(uc => uc.User)
                                            .Include(c => c.CoursesLecturers)
                                                    .ThenInclude(ul => ul.Lecturer)
                                            .FirstOrDefaultAsync(c => c.Id == course.Id);

            if (fetchedCourse == null)
            {
                throw new  NoContentException("Course not found");
            }

            var courseDto = new CourseGetDto
            {
                Id = fetchedCourse.Id,
                CourseName = fetchedCourse.CourseName,
                Faculty = fetchedCourse.Faculty != null
                              ? new FacultyOnlyDto
                              {
                                  Id = fetchedCourse.Faculty.Id,
                                  FacultyName = fetchedCourse.Faculty.FacultyName
                              }
                              : null,
                Lecturers = fetchedCourse.CoursesLecturers != null
                                   ? fetchedCourse.CoursesLecturers.Where(ul => ul.Lecturer != null).Select(c => new LecturerOnlyDto
                                   {
                                       Id = c.Lecturer.Id,
                                       Name = c.Lecturer.Name,
                                       SurName = c.Lecturer.SurName,
                                       Age = c.Lecturer.Age
                                   }).ToList()
                                   : new List<LecturerOnlyDto>(),
                Users = fetchedCourse.UsersCourses != null
                               ? fetchedCourse.UsersCourses.Where(uc => uc.User != null).Select(c => new UserOnlyDto
                               {
                                   Id = c.User.Id,
                                   Name = c.User.Name,
                                   SurName = c.User.SurName,
                                   Age = c.User.Age
                               }).ToList()
                               : new List<UserOnlyDto>()
            };

            var successResult = ApiResponse<CourseGetDto>.SuccesResult(courseDto);
            return successResult;

        }

        public async Task<ApiResponse<string>> UpdateCourseAsync(CoursePutDto input, CancellationToken cancellationToken)
        {
            var courseQueryable = await _universityContext.Courses.GetCoursesAsync(cancellationToken);
            var course = await courseQueryable.AsQueryable()
                                      .Include(c => c.UsersCourses)
                                            .ThenInclude(uc => uc.User)
                                      .Include(c => c.CoursesLecturers)
                                            .ThenInclude(cl => cl.Lecturer)
                                      .Where(c => c.Id == input.Id)
                                      .FirstOrDefaultAsync();

            if (input.Id == null)
            {
                throw new  BadRequestException("Course Id is null");
            }

            course.CourseName = input.CourseName;
            course.FacultyId = input.FacultyId.HasValue ? input.FacultyId : null;


            await _contextMethods.CourseRepository.UpdateCourseAsync(course, cancellationToken);


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
                foreach (var lecturerId in input.LecturerIds)
                {
                    course.CoursesLecturers.Clear();
                    course.CoursesLecturers.Add(new CoursesLecturersJoin()
                    {
                        LectureId = lecturerId,
                        CourseId = course.Id
                    });
                }
            }

            if (course == null)
            {
                throw new  NoContentException("Course not found");
            }

            await _contextMethods.CourseRepository.SaveChangesAsync(cancellationToken);
            var successResponse = ApiResponse<string>.SuccesResult("Course changed successfully");
            return successResponse;

        }

        public async Task<ApiResponse<string>> DeleteCourse(int courseId, CancellationToken cancellationToken)
        {
            var course = await _universityContext.Courses
                                 .Include(uc => uc.UsersCourses)
                                        .ThenInclude(u => u.User)
                                 .Include(cl => cl.CoursesLecturers)
                                        .ThenInclude(l => l.Lecturer)
                                 .FirstOrDefaultAsync(c => c.Id == courseId);
            if(course == null)
            {
                throw new  NotFoundException("Course not found on that Id");
            }

            if (await _contextMethods.CourseRepository.DeleteCourseAsync(courseId, cancellationToken) &&
                       await _contextMethods.CourseRepository.DeleteUsersCoursesAsync(courseId, cancellationToken) &&
                       await _contextMethods.CourseRepository.DeleteCourseLecturersAsync(courseId, cancellationToken))
            {
                await _contextMethods.CourseRepository.SaveChangesAsync(cancellationToken);
                var succesResult = ApiResponse<string>.SuccesResult("Course deleted successfully");
                return succesResult;
            }
            else
            {
                throw new Exception();
            }
            
            
        }
    }
}
