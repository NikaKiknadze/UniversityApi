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
        /*
        public List<UserGetDto> GetUserById(int userId)
        {
            var user = _userRepository.GetUsers()
                                      .Include(u => u.Faculty)
                                      .Include(u => u.UsersLecturers)
                                          .ThenInclude(ul => ul.Lecturer)
                                      .Include(u => u.UsersCourses)
                                          .ThenInclude(uc => uc.Course)
                                      .Where(u => u.Id == userId)
                                      .Select(u => new UserGetDto
                                      {
                                          Id = u.Id,
                                          Name = u.Name,
                                          SurName = u.SurName,
                                          Age = u.Age,
                                          Faculty = u.Faculty != null
                                                    ? new FacultyOnlyDto
                                                    {
                                                        Id = u.Faculty.Id,
                                                        FacultyName = u.Faculty.FacultyName
                                                    }
                                                    : null,
                                          Courses = u.UsersCourses != null
                                                    ? u.UsersCourses.Select(uc => new CourseOnlyDto
                                                    {
                                                        Id = uc.Course.Id,
                                                        CourseName = uc.Course.CourseName
                                                    }).ToList()
                                                    : new List<CourseOnlyDto>(),
                                          Lecturers = u.UsersLecturers != null
                                                      ? u.UsersLecturers.Select(ul => new LecturerOnlyDto
                                                      {
                                                          Id = ul.Lecturer.Id,
                                                          Name = ul.Lecturer.Name,
                                                          SurName = ul.Lecturer.SurName,
                                                          Age = ul.Lecturer.Age
                                                      }).ToList()
                                                      : new List<LecturerOnlyDto>()
                                      }).ToList();

            return user;
        }

        public List<UserGetDto> GetUsers()
        {
            var users = _userRepository.GetUsersWithRelatedData();

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

            return userDtos;
        }

        public UserGetDto CreateUser(UserPostDto input)
        {
            var user = new User
            {
                Name = input.Name,
                SurName = input.SurName,
                Age = (int)input.Age,
                FacultyId = input.FacultyId,
                UsersLecturers = new List<UsersLecturersJoin>(),
                UsersCourses = new List<UsersCoursesJoin>()
            };

            _userRepository.CreateUser(user);
            _userRepository.SaveChanges();

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

            return new UserGetDto
            {
                Id = user.Id,
                Name = user.Name,
                SurName = user.SurName,
                Age = user.Age,
                Faculty = user.FacultyId != null
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
            };
        }

        public bool UpdateUser(UserPutDto input)
        {
            var user = _userRepository.GetUsers()
                                      .Include(u => u.UsersCourses)
                                      .Include(u => u.UsersLecturers)
                                      .Where(u => u.Id == input.Id)
                                      .FirstOrDefault();
            user.Id = input.Id.HasValue ? (int)input.Id : 0;
            user.Name = input.Name;
            user.SurName = input.Surname;
            user.Age = input.Age.HasValue ? (int)input.Age : 0;
            user.FacultyId = input.FacultyId.HasValue ? (int)input.FacultyId : null;


            if (_userRepository.UpdateUser(user))
            {
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
                _userRepository.SaveChanges();
                return true;
            }

            return false;

        }

        public bool DeleteUser(int userId)
        {
            if (_userRepository.DeleteUser(userId) &&
                _userRepository.DeleteUsersCourses(userId) &&
                _userRepository.DeleteUsersLecturers(userId))
            {
                _userRepository.SaveChanges();
                return true;
            }
            return false;
        }
        */
        public ApiResponse<UserGetDto> GetUserById(int userId)
        {
            try
            {
                var user = _userRepository.GetUsers()
                                          .Include(u => u.Faculty)
                                          .Include(u => u.UsersLecturers)
                                              .ThenInclude(ul => ul.Lecturer)
                                          .Include(u => u.UsersCourses)
                                              .ThenInclude(uc => uc.Course)
                                          .FirstOrDefault(u => u.Id == userId);
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

        public ApiResponse<List<UserGetDto>> GetUsers()
        {
            try
            {
                var users = _userRepository.GetUsersWithRelatedData();

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

        public ApiResponse<UserGetDto> CreateUser(UserPostDto input)
        {
            try
            {
                var user = new User
                {
                    Name = input.Name,
                    SurName = input.SurName,
                    Age = (int)input.Age,
                    FacultyId = input.FacultyId,
                    UsersLecturers = new List<UsersLecturersJoin>(),
                    UsersCourses = new List<UsersCoursesJoin>()
                };

                _userRepository.CreateUser(user);
                _userRepository.SaveChanges();

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

                var userDto = new UserGetDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    SurName = user.SurName,
                    Age = user.Age,
                    Faculty = user.FacultyId != null
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
                };

                return new ApiResponse<UserGetDto>(true, "User created successfully", userDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserGetDto>(false, $"Error: {ex.Message}", null);
            }
        }

        public ApiResponse<bool> UpdateUser(UserPutDto input)
        {
            try
            {
                var user = _userRepository.GetUsers()
                                          .Include(u => u.UsersCourses)
                                          .Include(u => u.UsersLecturers)
                                          .Where(u => u.Id == input.Id)
                                          .FirstOrDefault();

                if (user == null)
                {
                    return new ApiResponse<bool>(false, "User not found", false);
                }

                user.Id = input.Id.HasValue ? (int)input.Id : 0;
                user.Name = input.Name;
                user.SurName = input.Surname;
                user.Age = input.Age.HasValue ? (int)input.Age : 0;
                user.FacultyId = input.FacultyId.HasValue ? (int)input.FacultyId : null;

                _userRepository.UpdateUser(user);

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

                _userRepository.SaveChanges();
                return new ApiResponse<bool>(true, "User updated successfully", true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }

        public ApiResponse<bool> DeleteUser(int userId)
        {
            try
            {
                if (_userRepository.DeleteUser(userId) &&
                    _userRepository.DeleteUsersCourses(userId) &&
                    _userRepository.DeleteUsersLecturers(userId))
                {
                    _userRepository.SaveChanges();
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
