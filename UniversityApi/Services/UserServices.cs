using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Xml;
using UniversityApi.CustomResponses;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repositories;

namespace UniversityApi.Services
{
    public class UserServices
    {
        private readonly UniversistyContext _context;
        private readonly UserRepository _userRepository;

        public UserServices(UserRepository userRepository, UniversistyContext context)
        {
            _context = context;
            _userRepository = userRepository;
        }
        
        public async Task<ApiResponse<UserGetDto>> GetUserByIdAsync(int userId)
        {
            try
            {
                var userQueryable = await _userRepository.GetUsersAsync();
                var user = await userQueryable
                                          .Include(u => u.Faculty)
                                          .Include(u => u.UsersLecturers)
                                              .ThenInclude(ul => ul.Lecturer)
                                          .Include(u => u.UsersCourses)
                                              .ThenInclude(uc => uc.Course)
                                          .FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return new ApiResponse<UserGetDto>(false, "User not found", null);
                }

                var userDto = new UserGetDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    SurName = user.SurName,
                    Age = user.Age,
                    Faculty = user.Faculty != null
                              ? new FacultyOnlyDto
                              {
                                  Id = user.Faculty.Id,
                                  FacultyName = user.Faculty.FacultyName
                              }
                              : null,
                    Courses = user.UsersCourses != null
                              ? user.UsersCourses.Select(uc => new CourseOnlyDto
                              {
                                  Id = uc.Course.Id,
                                  CourseName = uc.Course.CourseName
                              }).ToList()
                              : new List<CourseOnlyDto>(),
                    Lecturers = user.UsersLecturers != null
                                ? user.UsersLecturers.Select(ul => new LecturerOnlyDto
                                {
                                    Id = ul.Lecturer.Id,
                                    Name = ul.Lecturer.Name,
                                    SurName = ul.Lecturer.SurName,
                                    Age = ul.Lecturer.Age
                                }).ToList()
                                : new List<LecturerOnlyDto>()
                };

                return new ApiResponse<UserGetDto>(true, "User fetched successfully", userDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserGetDto>(false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<List<UserGetDto>>> GetUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetUsersWithRelatedDataAsync();

                var userDtos = users.Select(user => new UserGetDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    SurName = user.SurName,
                    Age = user.Age,
                    Faculty = user.Faculty != null
                              ? new FacultyOnlyDto
                              {
                                  Id = user.Faculty.Id,
                                  FacultyName = user.Faculty.FacultyName
                              }
                              : null,
                    Courses = user.UsersCourses != null
                              ? user.UsersCourses.Where(uc => uc.Course != null).Select(uc => new CourseOnlyDto
                              {
                                  Id = uc.Course.Id,
                                  CourseName = uc.Course.CourseName
                              }).ToList()
                              : new List<CourseOnlyDto>(),
                    Lecturers = user.UsersLecturers != null
                                ? user.UsersLecturers.Where(ul => ul.Lecturer != null).Select(ul => new LecturerOnlyDto
                                {
                                    Id = ul.Lecturer.Id,
                                    Name = ul.Lecturer.Name,
                                    SurName = ul.Lecturer.SurName,
                                    Age = ul.Lecturer.Age
                                }).ToList()
                                : new List<LecturerOnlyDto>()
                }).ToList();

                return new ApiResponse<List<UserGetDto>>(true, "Users fetched successfully", userDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<UserGetDto>>(false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<UserGetDto>> CreateUserAsync(UserPostDto input)
        {
            try
            {
                var user = new User
                {
                    Name = input.Name,
                    SurName = input.SurName,
                    Age = (int)input.Age,
                    FacultyId = input.FacultyId
                };

                

                if (!input.CourseIds.IsNullOrEmpty())
                {
                    user.UsersCourses = new List<UsersCoursesJoin>();

                    foreach (var courseId in input.CourseIds)
                    {
                        user.UsersCourses.Add(new UsersCoursesJoin()
                        {
                            CourseId = courseId,
                            UserId = user.Id
                        });
                    }
                }

                if (!input.LecturerIds.IsNullOrEmpty())
                {
                    user.UsersLecturers = new List<UsersLecturersJoin>();

                    foreach (var lecturerId in input.LecturerIds)
                    {
                        user.UsersLecturers.Add(new UsersLecturersJoin()
                        {
                            LecturerId = lecturerId,
                            UserId = user.Id
                        });
                    }
                }

                await _userRepository.CreateUserAsync(user);
                await _userRepository.SaveChangesAsync();


                var userQueryable = await _userRepository.GetUsersAsync();
                var fetchedUser = await userQueryable
                                          .Include(u => u.Faculty)
                                          .Include(u => u.UsersLecturers)
                                              .ThenInclude(ul => ul.Lecturer)
                                          .Include(u => u.UsersCourses)
                                              .ThenInclude(uc => uc.Course)
                                          .FirstOrDefaultAsync(u => u.Id == user.Id);
                if (fetchedUser == null)
                {
                    return new ApiResponse<UserGetDto>(false, "User not found", null);
                }

                var userDto = new UserGetDto
                {
                    Id = fetchedUser.Id,
                    Name = fetchedUser.Name,
                    SurName = fetchedUser.SurName,
                    Age = fetchedUser.Age,
                    Faculty = fetchedUser.Faculty != null
                              ? new FacultyOnlyDto
                              {
                                  Id = fetchedUser.Faculty.Id,
                                  FacultyName = fetchedUser.Faculty.FacultyName
                              }
                              : null,
                    Courses = fetchedUser.UsersCourses != null
                              ? fetchedUser.UsersCourses.Select(uc => new CourseOnlyDto
                              {
                                  Id = uc.Course.Id,
                                  CourseName = uc.Course.CourseName
                              }).ToList()
                              : new List<CourseOnlyDto>(),
                    Lecturers = fetchedUser.UsersLecturers != null
                                ? fetchedUser.UsersLecturers.Select(ul => new LecturerOnlyDto
                                {
                                    Id = ul.Lecturer.Id,
                                    Name = ul.Lecturer.Name,
                                    SurName = ul.Lecturer.SurName,
                                    Age = ul.Lecturer.Age
                                }).ToList()
                                : new List<LecturerOnlyDto>()
                };

                
                

                return new ApiResponse<UserGetDto>(true, "User created successfully", userDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserGetDto>(false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<bool>> UpdateUserAsync(UserPutDto input)
        {
            try
            {
                var userQueryable = await _userRepository.GetUsersAsync();
                var user = await userQueryable.AsQueryable()
                                          .Include(u => u.UsersCourses)
                                          .Include(u => u.UsersLecturers)
                                          .Where(u => u.Id == input.Id)
                                          .FirstOrDefaultAsync();

                

                user.Id = input.Id.HasValue ? (int)input.Id : 0;
                user.Name = input.Name;
                user.SurName = input.Surname;
                user.Age = input.Age.HasValue ? (int)input.Age : 0;
                user.FacultyId = input.FacultyId.HasValue ? (int)input.FacultyId : null;

                

                user.UsersCourses.Clear();
                if (!input.CourseIds.IsNullOrEmpty())
                {
                    foreach (var courseId in input.CourseIds)
                    {
                        user.UsersCourses.Add(new UsersCoursesJoin
                        {
                            CourseId = courseId,
                            UserId = user.Id
                        });
                    }
                }

                user.UsersLecturers.Clear();
                if (!input.LecturerIds.IsNullOrEmpty())
                {
                    foreach (var lecturerId in input.LecturerIds)
                    {
                        user.UsersLecturers.Add(new UsersLecturersJoin
                        {
                            LecturerId = lecturerId,
                            UserId = user.Id
                        });
                    }
                }

                if (user == null)
                {
                    return new ApiResponse<bool>(false, "User not found", false);
                }

                await _userRepository.UpdateUserAsync(user);
                await _userRepository.SaveChangesAsync();
                return new ApiResponse<bool>(true, "User updated successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int userId)
        {
            try
            {
                if (await _userRepository.DeleteUserAsync(userId) &&
                    await _userRepository.DeleteUsersCoursesAsync(userId) &&
                    await _userRepository.DeleteUsersLecturers(userId))
                {
                    await _userRepository.SaveChangesAsync();
                    return new ApiResponse<bool>(true, "User deleted successfully", true);
                }
                return new ApiResponse<bool>(false, "Failed to delete User", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }
    }

}
