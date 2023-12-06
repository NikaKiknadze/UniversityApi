using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Xml;
using UniversityApi.CustomResponses;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repository;
using UniversityApi.Repository.Repositoryes;
using UniversityApi.Service.ServiceAbstracts;

namespace UniversityApi.Service.Services
{
    public class UserServices : IUserServices
    {
        private readonly UniversistyContext _context;
        private readonly IRepositories _repositories;

        public UserServices(IRepositories userRepository, UniversistyContext context)
        {
            _context = context;
            _repositories = userRepository;
        }

        public async Task<ApiResponse<UserGetDto>> GetUserByIdAsync(int userId)
        {
            var userQueryable = await _repositories.UserRepository.GetUsersAsync();
            var user = await userQueryable
                                      .Include(u => u.Faculty)
                                      .Include(u => u.UsersLecturers)
                                          .ThenInclude(ul => ul.Lecturer)
                                      .Include(u => u.UsersCourses)
                                          .ThenInclude(uc => uc.Course)
                                      .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new CustomExceptions.NotFoundException("Users not found");
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

            var successResult = ApiResponse<UserGetDto>.SuccesResult(userDto);
            return successResult;
        }

        public async Task<ApiResponse<List<UserGetDto>>> GetUsersAsync()
        {
            var users = await _repositories.UserRepository.GetUsersWithRelatedDataAsync();

            if (users == null)
            {
                throw new CustomExceptions.NotFoundException("User not found");
            }

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

            var successResult = ApiResponse<List<UserGetDto>>.SuccesResult(userDtos);
            return successResult;
        }

        public async Task<ApiResponse<UserGetDto>> CreateUserAsync(UserPostDto input)
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

            await _repositories.UserRepository.CreateUserAsync(user);
            await _repositories.UserRepository.SaveChangesAsync();


            var userQueryable = await _repositories.UserRepository.GetUsersAsync();
            var fetchedUser = await userQueryable
                                      .Include(u => u.Faculty)
                                      .Include(u => u.UsersLecturers)
                                          .ThenInclude(ul => ul.Lecturer)
                                      .Include(u => u.UsersCourses)
                                          .ThenInclude(uc => uc.Course)
                                      .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (fetchedUser == null)
            {
                throw new CustomExceptions.NotFoundException("User not found");
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




            var sucessResult = ApiResponse<UserGetDto>.SuccesResult(userDto);
            return sucessResult;
        }

        public async Task<ApiResponse<string>> UpdateUserAsync(UserPutDto input)
        {
            var userQueryable = await _repositories.UserRepository.GetUsersAsync();
            var user = await userQueryable.AsQueryable()
                                      .Include(u => u.UsersCourses)
                                      .Include(u => u.UsersLecturers)
                                      .Where(u => u.Id == input.Id)
                                      .FirstOrDefaultAsync();



            user.Id = input.Id.HasValue ? (int)input.Id : 0;
            user.Name = input.Name;
            user.SurName = input.Surname;
            user.Age = input.Age.HasValue ? (int)input.Age : 0;
            user.FacultyId = input.FacultyId.HasValue ? input.FacultyId : null;

            await _repositories.UserRepository.UpdateUserAsync(user);


            if (!input.CourseIds.IsNullOrEmpty())
            {
                foreach (var courseId in input.CourseIds)
                {
                    user.UsersCourses.Clear();
                    user.UsersCourses.Add(new UsersCoursesJoin
                    {
                        CourseId = courseId,
                        UserId = user.Id
                    });
                }
            }


            if (!input.LecturerIds.IsNullOrEmpty())
            {
                foreach (var lecturerId in input.LecturerIds)
                {
                    user.UsersLecturers.Clear();
                    user.UsersLecturers.Add(new UsersLecturersJoin
                    {
                        LecturerId = lecturerId,
                        UserId = user.Id
                    });
                }
            }

            if (user == null)
            {
                throw new CustomExceptions.NotFoundException("User not found");
            }


            await _repositories.UserRepository.SaveChangesAsync();

            var successResult = ApiResponse<string>.SuccesResult("Course changed successfully");
            return successResult;

        }

        public async Task<ApiResponse<string>> DeleteUserAsync(int userId)
        {
            var user = GetUserByIdAsync(userId);

            if(user == null)
            {
                throw new CustomExceptions.NotFoundException("User not found on that Id");
            }

            if (await _repositories.UserRepository.DeleteUserAsync(userId) &&
                    await _repositories.UserRepository.DeleteUsersCoursesAsync(userId) &&
                    await _repositories.UserRepository.DeleteUsersLecturers(userId))
            {
                await _repositories.UserRepository.SaveChangesAsync();
                var successResult = ApiResponse<string>.SuccesResult("User deleted successfully");
                return successResult;
            }
            else
            {
                throw new Exception();
            }
        }
    }

}
