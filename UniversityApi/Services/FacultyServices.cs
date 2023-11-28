using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repositories;
using UniversityApi.CustomResponses;
using Microsoft.VisualBasic;

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

        public async Task<ApiResponse<FacultyGetDto>> GetFacultyByIdAsync(int facultyId)
        {
            try
            {

                var facultyQueryable = await _facultyRepository.GetFacultiesAsync();
                var faculty = await facultyQueryable.AsQueryable()
                                                .Include(f => f.Users)
                                                    .ThenInclude(u => u.UsersCourses)
                                                        .ThenInclude(uc => uc.Course)
                                                .Include(f => f.Users)
                                                    .ThenInclude(u => u.UsersLecturers)
                                                        .ThenInclude(ul => ul.Lecturer)
                                                .Include(f => f.Courses)
                                                .FirstOrDefaultAsync(f => f.Id == facultyId);
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

        public async Task<ApiResponse<List<FacultyGetDto>>> GetFacultiesAsync()
        {
            try
            {

                var faculties = await _facultyRepository.GetFacultiesWithRelatedData();

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

        public async Task<ApiResponse<FacultyGetDto>> CreateFacultyAsync(FacultyPostDto input)
        {
            try
            {
                var faculty = new Faculty
                {
                    FacultyName = input.FacultyName,
                    Users = new List<User>(),
                    Courses = new List<Course>()
                };


                await _facultyRepository.CreateFacultyAsync(faculty);



                if (!input.UserIds.IsNullOrEmpty())
                {
                    foreach (var userId in input.UserIds)
                    {
                        var user = await _userRepository.GetUserByIdAsync(userId);
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
                        var course = await _courseRepository.GetCourseByIdAsync(courseId);

                        if (course != null)
                        {
                            course.FacultyId = faculty.Id;
                            faculty.Courses.Add(course);

                        }
                    }
                }

                await _userRepository.SaveChangesAsync();
                await _facultyRepository.SaveChangesAsync();

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

        public async Task<ApiResponse<bool>> UpdateFacultyAsync(FacultyPutDto input)
        {
            try
            {
                var facultyQueryable = await _facultyRepository.GetFacultiesAsync();
                var faculty = await facultyQueryable.AsQueryable()
                    .Include(f => f.Users)
                    .Include(f => f.Courses)
                    .Where(f => f.Id == input.Id)
                    .FirstOrDefaultAsync();

                if (faculty == null)
                {
                    return new ApiResponse<bool>(false, "Faculty not found", false);
                }

                faculty.Id = input.Id.HasValue ? (int)input.Id.Value : 0;
                faculty.FacultyName = input.FacultyName;

                if (await _facultyRepository.UpdateFacultyAsync(faculty))
                {
                    if (!input.UserIds.IsNullOrEmpty())
                    {
                        faculty.Users.Clear();
                        foreach (var userId in input.UserIds)
                        {
                            var user = await _userRepository.GetUserByIdAsync(userId);
                            faculty.Users.Add(user);
                        }
                    }

                    if (!input.CourseIds.IsNullOrEmpty())
                    {
                        faculty.Courses.Clear();
                        foreach (var courseId in input.CourseIds)
                        {
                            var course = await _courseRepository.GetCourseByIdAsync(courseId);
                            faculty.Courses.Add(course);
                        }
                    }

                    await _facultyRepository.SaveChangesAsync();
                    return new ApiResponse<bool>(true, "Faculty updated successfully", true);
                }
                return new ApiResponse<bool>(false, "Error updating faculty", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<bool>> DeleteFacultyAsync(int facultyId)
        {
            try
            {
                if (await _facultyRepository.DeleteFacultyAsync(facultyId))
                {
                    await _facultyRepository.SaveChangesAsync();
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
