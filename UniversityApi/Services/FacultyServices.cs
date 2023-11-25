using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repositories;
using UniversityApi.CustomResponses;

namespace UniversityApi.Services
{
    public class FacultyServices
    {
        public readonly UniversistyContext _context;
        public readonly FacultyRepository _facultyRepository;
        public readonly UserRepository _userRepository;
        public readonly CourseRepository _courseRepository;

        public FacultyServices(UniversistyContext context, FacultyRepository facultyRepository, UserRepository userRepository, CourseRepository courseRepository)
        {
            _context = context;
            _facultyRepository = facultyRepository;
            _userRepository = userRepository;
            _courseRepository = courseRepository;
        }

        public ApiResponse<FacultyGetDto> GetFacultyById(int facultyId)
        {
            try
            {

                var faculty = _facultyRepository.GetFaculties()
                                                .Include(f => f.Users)
                                                    .ThenInclude(u => u.UsersCourses)
                                                        .ThenInclude(uc => uc.Course)
                                                .Include(f => f.Users)
                                                    .ThenInclude(u => u.UsersLecturers)
                                                        .ThenInclude(ul => ul.Lecturer)
                                                .Include(f => f.Courses)
                                                .FirstOrDefault(f => f.Id == facultyId);
                if (faculty == null)
                {
                    return new ApiResponse<FacultyGetDto>(false, "Faculty not found", null);
                }
                var facultyDto = new FacultyGetDto
                {
                    Id = faculty.Id,
                    FacultyName = faculty.FacultyName,
                    Users = faculty.Users != null
                              ? faculty.Users.Select(u => new UserOnlyDto
                              {
                                  Id = u.Id,
                                  Name = u.Name,
                                  SurName = u.SurName,
                                  Age = u.Age
                              }).ToList()
                              : new List<UserOnlyDto>(),
                    Courses = faculty.Courses != null
                                ? faculty.Courses.Select(uc => new CourseOnlyDto
                                {
                                    Id = uc.Id,
                                    CourseName = uc.CourseName
                                }).ToList()
                                : new List<CourseOnlyDto>()
                };
                return new ApiResponse<FacultyGetDto>(true, "Faculty fetched successfully", facultyDto);

            }
            catch (Exception ex)
            {
                return new ApiResponse<FacultyGetDto>(false, $"Error: {ex.Message}", null);
            }
        }

        public ApiResponse<List<FacultyGetDto>> GetFaculties()
        {
            try
            {
                var faculties = _facultyRepository.GetFaculties()
                    .Include(f => f.Users)
                        .ThenInclude(u => u.UsersCourses)
                            .ThenInclude(uc => uc.Course)
                    .Include(f => f.Users)
                        .ThenInclude(u => u.UsersLecturers)
                            .ThenInclude(ul => ul.Lecturer)
                    .Include(f => f.Courses)
                    .ToList();

                var facultyDtos = faculties.Select(f => new FacultyGetDto
                {
                    Id = f.Id,
                    FacultyName = f.FacultyName,
                    Users = f.Users != null
                        ? f.Users.Select(u => new UserOnlyDto
                        {
                            Id = u.Id,
                            Name = u.Name,
                            SurName = u.SurName,
                            Age = u.Age
                        }).ToList()
                        : new List<UserOnlyDto>(),
                    Courses = f.Courses != null
                        ? f.Courses.Select(uc => new CourseOnlyDto
                        {
                            Id = uc.Id,
                            CourseName = uc.CourseName
                        }).ToList()
                        : new List<CourseOnlyDto>()
                }).ToList();

                return new ApiResponse<List<FacultyGetDto>>(true, "Faculties fetched successfully", facultyDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<FacultyGetDto>>(false, $"Error: {ex.Message}", null);
            }

        }

        public ApiResponse<FacultyGetDto> CreateFaculty(FacultyPostDto input)
        {
            try
            {
                var faculty = new Faculty
                {
                    FacultyName = input.FacultyName,
                    Users = new List<User>(),
                    Courses = new List<Course>()
                };


                _facultyRepository.CreateFaculty(faculty);



                if (!input.UserIds.IsNullOrEmpty())
                {
                    foreach (var userId in input.UserIds)
                    {
                        var user = _userRepository.GetUserById(userId);
                        if (user != null)
                        {
                            user.FacultyId = faculty.Id;
                            faculty.Users.Add(user);
                        }
                    }
                }

                if (!input.CourseIds.IsNullOrEmpty())
                {
                    foreach (var courseId in input.CourseIds)
                    {
                        var course = _courseRepository.GetCourseById(courseId);

                        if (course != null)
                        {
                            course.FacultyId = faculty.Id;
                            faculty.Courses.Add(course);

                        }
                    }
                }

                _userRepository.SaveChanges();
                _facultyRepository.SaveChanges();

                var facultyDto = new FacultyGetDto
                {
                    Id = faculty.Id,
                    FacultyName = faculty.FacultyName,
                    Users = faculty.Users != null
                                    ? faculty.Users.Select(u => new UserOnlyDto
                                    {
                                        Id = u.Id,
                                        Name = u.Name,
                                        SurName = u.SurName,
                                        Age = u.Age
                                    }).ToList()
                                    : new List<UserOnlyDto>(),
                    Courses = faculty.Courses != null
                                ? faculty.Courses.Select(uc => new CourseOnlyDto
                                {
                                    Id = uc.Id,
                                    CourseName = uc.CourseName
                                }).ToList()
                                : new List<CourseOnlyDto>()
                };

                return new ApiResponse<FacultyGetDto>(true, "Faculty created successfully", facultyDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<FacultyGetDto>(false, $"Error: {ex.Message}", null);
            }

        }

        public ApiResponse<bool> UpdateFaculty(FacultyPutDto input)
        {
            try
            {
                var faculty = _facultyRepository.GetFaculties()
                    .Include(f => f.Users)
                    .Include(f => f.Courses)
                    .Where(f => f.Id == input.Id)
                    .FirstOrDefault();

                if (faculty == null)
                {
                    return new ApiResponse<bool>(false, "Faculty not found", false);
                }

                faculty.Id = input.Id.HasValue ? (int)input.Id.Value : 0;
                faculty.FacultyName = input.FacultyName;

                if (_facultyRepository.UpdateFaculty(faculty))
                {
                    if (!input.UserIds.IsNullOrEmpty())
                    {
                        faculty.Users.Clear();
                        foreach (var userId in input.UserIds)
                        {
                            var user = _userRepository.GetUserById(userId);
                            faculty.Users.Add(user);
                        }
                    }

                    if (!input.CourseIds.IsNullOrEmpty())
                    {
                        faculty.Courses.Clear();
                        foreach (var courseId in input.CourseIds)
                        {
                            var course = _courseRepository.GetCourseById(courseId);
                            faculty.Courses.Add(course);
                        }
                    }

                    _facultyRepository.SaveChanges();
                    return new ApiResponse<bool>(true, "Faculty updated successfully", true);
                }
                return new ApiResponse<bool>(false, "Error updating faculty", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }

        public ApiResponse<bool> DeleteFaculty(int facultyId)
        {
            try
            {
                if (_facultyRepository.DeleteFaculty(facultyId))
                {
                    _facultyRepository.SaveChanges();
                    return new ApiResponse<bool>(true, "Faculty deleted successfully", true);
                }
                return new ApiResponse<bool>(false, "Failed to delete Faculty", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }

        }
    }
}
