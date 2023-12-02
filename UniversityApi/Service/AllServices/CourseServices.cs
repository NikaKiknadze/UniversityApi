using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.CustomResponses;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repository;
using UniversityApi.Service.ServiceAbstracts;

namespace UniversityApi.Service.Services
{
    public class CourseServices : ICourseServices
    {
        private readonly UniversistyContext _context;
        private readonly IRepositories _repositories;

        public CourseServices(UniversistyContext context, IRepositories repository)
        {
            _context = context;
            _repositories = repository;
        }

        public async Task<ApiResponse<List<CourseGetDto>>> GetCoursesAsync()
        {
            try
            {
                var courses = await _repositories.CourseRepository.GetCoursesWithRelatedDataAsync();

                var courseDtos = courses.Select(course => new CourseGetDto
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
                }).ToList();

                return new ApiResponse<List<CourseGetDto>>(true, "Courses fetched successfully", courseDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CourseGetDto>>(false, $"Error : {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<CourseGetDto>> CreateCourseAsync(CoursePostDto input)
        {
            try
            {
                var course = new Course
                {
                    CourseName = input.CourseName,
                    FacultyId = input.FacultyId
                };

                if (!input.UserIds.IsNullOrEmpty())
                {
                    course.UsersCourses = new List<UsersCoursesJoin>();

                    foreach (var userId in input.UserIds)
                    {
                        course.UsersCourses.Add(new UsersCoursesJoin()
                        {
                            UserId = userId,
                            CourseId = course.Id
                        });
                    }
                }

                if (!input.LecturerIds.IsNullOrEmpty())
                {
                    course.CoursesLecturers = new List<CoursesLecturersJoin>();

                    foreach (var lecturerId in input.LecturerIds)
                    {
                        course.CoursesLecturers.Add(new CoursesLecturersJoin()
                        {
                            LectureId = lecturerId,
                            CourseId = course.Id
                        });
                    }
                }

                await _repositories.CourseRepository.CreateCourseAsync(course);
                await _repositories.CourseRepository.SaveChangesAsync();

                var courseQueryable = await _repositories.CourseRepository.GetCoursesAsync();
                var fetchedCourse = await courseQueryable
                                                .Include(c => c.Faculty)
                                                .Include(c => c.UsersCourses)
                                                        .ThenInclude(uc => uc.User)
                                                .Include(c => c.CoursesLecturers)
                                                        .ThenInclude(ul => ul.Lecturer)
                                                .FirstOrDefaultAsync(c => c.Id == course.Id);

                if (fetchedCourse == null)
                {
                    return new ApiResponse<CourseGetDto>(false, "Course not found", null);
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

                return new ApiResponse<CourseGetDto>(true, "Course created successfully", courseDto);

            }
            catch (Exception ex)
            {
                return new ApiResponse<CourseGetDto>(false, $"Error: {ex.Message}", null);
            }

        }

        public async Task<ApiResponse<bool>> UpdateCourseAsync(CoursePutDto input)
        {
            try
            {
                var courseQueryable = await _repositories.CourseRepository.GetCoursesAsync();
                var course = await courseQueryable.AsQueryable()
                                          .Include(c => c.UsersCourses)
                                                .ThenInclude(uc => uc.User)
                                          .Include(c => c.CoursesLecturers)
                                                .ThenInclude(cl => cl.Lecturer)
                                          .Where(c => c.Id == input.Id)
                                          .FirstOrDefaultAsync();


                course.Id = input.Id.HasValue ? (int)input.Id : 0;
                course.CourseName = input.CourseName;
                course.FacultyId = input.FacultyId.HasValue ? input.FacultyId : null;


                await _repositories.CourseRepository.UpdateCourseAsync(course);


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


                await _repositories.CourseRepository.SaveChangesAsync();
                return new ApiResponse<bool>(true, "Course changed Successfully", true);

            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }

        }

        public async Task<ApiResponse<bool>> DeleteCourse(int courseId)
        {
            try
            {
                if (await _repositories.CourseRepository.DeleteCourseAsync(courseId) &&
                       await _repositories.CourseRepository.DeleteUsersCoursesAsync(courseId) &&
                       await _repositories.CourseRepository.DeleteCourseLecturersAsync(courseId))
                {
                    await _repositories.CourseRepository.SaveChangesAsync();
                    return new ApiResponse<bool>(true, "Course deleted successfully", true);
                }
                return new ApiResponse<bool>(false, "Failed to delete Course", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }

        }
    }
}
