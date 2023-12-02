using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.CustomResponses;
using Microsoft.VisualBasic;
using UniversityApi.Repository.Repositoryes;
using UniversityApi.Repository;
using UniversityApi.Service.ServiceAbstracts;

namespace UniversityApi.Service.Services
{
    public class FacultyServices : IFacultyServices
    {
        public readonly UniversistyContext _context;
        public readonly IRepositories _repositories;

        public FacultyServices(UniversistyContext context, IRepositories repositories)
        {
            _context = context;
            _repositories = repositories;
        }

        public async Task<ApiResponse<FacultyGetDto>> GetFacultyByIdAsync(int facultyId)
        {
            try
            {

                var facultyQueryable = await _repositories.FacultyRepository.GetFacultiesAsync();
                var faculty = await facultyQueryable
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

                var faculties = await _repositories.FacultyRepository.GetFacultiesWithRelatedDataAsync();

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
                    FacultyName = input.FacultyName
                };


                await _repositories.FacultyRepository.CreateFacultyAsync(faculty);



                if (!input.UserIds.IsNullOrEmpty())
                {
                    faculty.Users = new List<User>();

                    foreach (var userId in input.UserIds)
                    {
                        var user = await _repositories.UserRepository.GetUserByIdAsync(userId);
                        if (user != null)
                        {
                            user.FacultyId = faculty.Id;
                            faculty.Users.Add(user);
                        }
                    }
                }

                if (!input.CourseIds.IsNullOrEmpty())
                {
                    faculty.Courses = new List<Course>();

                    foreach (var courseId in input.CourseIds)
                    {
                        var course = await _repositories.CourseRepository.GetCourseByIdAsync(courseId);

                        if (course != null)
                        {
                            course.FacultyId = faculty.Id;
                            faculty.Courses.Add(course);

                        }
                    }
                }

                await _repositories.UserRepository.SaveChangesAsync();
                await _repositories.FacultyRepository.SaveChangesAsync();

                var facultyQueryable = await _repositories.FacultyRepository.GetFacultiesAsync();
                var fetchedFaculty = await facultyQueryable
                                                .Include(f => f.Users)
                                                    .ThenInclude(u => u.UsersCourses)
                                                        .ThenInclude(uc => uc.Course)
                                                .Include(f => f.Users)
                                                    .ThenInclude(u => u.UsersLecturers)
                                                        .ThenInclude(ul => ul.Lecturer)
                                                .Include(f => f.Courses)
                                                .FirstOrDefaultAsync(f => f.Id == faculty.Id);
                if (fetchedFaculty == null)
                {
                    return new ApiResponse<FacultyGetDto>(false, "Faculty not found", null);
                }

                var facultyDto = new FacultyGetDto
                {
                    Id = fetchedFaculty.Id,
                    FacultyName = fetchedFaculty.FacultyName,
                    Users = fetchedFaculty.Users != null
                                    ? fetchedFaculty.Users.Select(u => new UserOnlyDto
                                    {
                                        Id = u.Id,
                                        Name = u.Name,
                                        SurName = u.SurName,
                                        Age = u.Age
                                    }).ToList()
                                    : new List<UserOnlyDto>(),
                    Courses = fetchedFaculty.Courses != null
                                ? fetchedFaculty.Courses.Select(uc => new CourseOnlyDto
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
                var facultyQueryable = await _repositories.FacultyRepository.GetFacultiesAsync();
                var faculty = await facultyQueryable.AsQueryable()
                    .Include(f => f.Users)
                    .Include(f => f.Courses)
                    .Where(f => f.Id == input.Id)
                    .FirstOrDefaultAsync();



                faculty.Id = input.Id.HasValue ? input.Id.Value : 0;
                faculty.FacultyName = input.FacultyName;

                if (await _repositories.FacultyRepository.UpdateFacultyAsync(faculty))
                {
                    if (faculty == null)
                    {
                        return new ApiResponse<bool>(false, "Faculty not found", false);
                    }

                    if (!input.UserIds.IsNullOrEmpty())
                    {
                        faculty.Users.Clear();
                        foreach (var userId in input.UserIds)
                        {
                            var user = await _repositories.UserRepository.GetUserByIdAsync(userId);
                            faculty.Users.Add(user);
                        }
                    }

                    if (!input.CourseIds.IsNullOrEmpty())
                    {
                        faculty.Courses.Clear();
                        foreach (var courseId in input.CourseIds)
                        {
                            var course = await _repositories.CourseRepository.GetCourseByIdAsync(courseId);
                            faculty.Courses.Add(course);
                        }
                    }

                    await _repositories.FacultyRepository.SaveChangesAsync();
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
                if (await _repositories.FacultyRepository.DeleteFacultyAsync(facultyId))
                {
                    await _repositories.FacultyRepository.SaveChangesAsync();
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
