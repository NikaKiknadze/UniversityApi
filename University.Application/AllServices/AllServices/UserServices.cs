using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using University.Application.AllServices.ServiceAbstracts;
using University.Data;
using University.Data.ContextMethodsDirectory;
using University.Data.Data;
using University.Data.Data.Entities;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.AllServices
{
    public class UserServices : IUserServices
    {
        private readonly UniversityContext _context;
        private readonly ContextMethods _contextMethods;
        private readonly IConfiguration _configuration;

        public UserServices(ContextMethods userContextMethods, UniversityContext context, IConfiguration configuration)
        {
            _context = context;
            _contextMethods = userContextMethods;
            _configuration = configuration;
        }
        public async Task<ApiResponse<GetDtoWithCount<List<UserGetDto>>>>GetUsersAsync(UserGetFilter filter, CancellationToken cancellationToken)
        {
            var users = await _contextMethods.UserRepository.GetUsersWithRelatedDataAsync(cancellationToken);
            
            if (users == null)
            {
                throw new  NotFoundException("User not found");
            }

            var filteredUsers = FilterData(filter, users);

            if (filteredUsers.Count == 0)
            {
                throw new  NotFoundException("User not found");
            }

            var userDtos = filteredUsers.Select(user => new UserGetDto
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
            })
                .OrderByDescending(u => u.Id)
                .Skip(filter.Offset ?? 0)
                .Take(filter.Limit ?? 10)
                .ToList();
            return ApiResponse<GetDtoWithCount<List<UserGetDto>>>.SuccesResult(new GetDtoWithCount<List<UserGetDto>>
            {
                Data = userDtos,
                Count = filteredUsers.Count()
            }) ;
        }

        public List<User> FilterData(UserGetFilter filter, IQueryable<User> users)
        {
            if (filter.Id != null)
            {
                users = users.Where(u => u.Id == filter.Id);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                users = users.Where(u => u.Name.Contains(filter.Name));
            }
            if(!filter.SurName.IsNullOrEmpty())
            {
                users = users.Where(u => u.SurName.Contains(filter.SurName));
            }
            if(filter.Age != null)
            {
                users = users.Where(u => u.Age == filter.Age);
            }
            if(filter.FacultyId != null)
            {
                users = users.Where(u => u.FacultyId == filter.FacultyId);
            }
            if (filter.CourseIds != null && filter.CourseIds.Any())
            {
                users = users.Where(u => u.UsersCourses
                             .Select(c => c.CourseId)
                             .Any(courseId => filter.CourseIds.Contains(courseId)));
            }
            if(filter.LecturerIds != null && filter.LecturerIds.Any())
            {
                users = users.Where(u => u.UsersLecturers
                             .Select(l => l.LecturerId)
                             .Any(lecturerId => filter.LecturerIds.Contains(lecturerId)));
            }
            return users.ToList();

        }

        public async Task<ApiResponse<UserGetDto>> CreateUserAsync(UserPostDto input, CancellationToken cancellationToken)
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

            await _contextMethods.UserRepository.CreateUserAsync(user, cancellationToken);
            await _contextMethods.UserRepository.SaveChangesAsync(cancellationToken);


            var userQueryable = await _contextMethods.UserRepository.GetUsersAsync(cancellationToken);
            var fetchedUser = await userQueryable
                                      .Include(u => u.Faculty)
                                      .Include(u => u.UsersLecturers)
                                          .ThenInclude(ul => ul.Lecturer)
                                      .Include(u => u.UsersCourses)
                                          .ThenInclude(uc => uc.Course)
                                      .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (fetchedUser == null)
            {
                throw new  NotFoundException("User not found");
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

        public async Task<ApiResponse<string>> UpdateUserAsync(UserPutDto input, CancellationToken cancellationToken)
        {
            var userQueryable = await _contextMethods.UserRepository.GetUsersAsync(cancellationToken);
            var user = await userQueryable.AsQueryable()
                                      .Include(u => u.UsersCourses)
                                      .Include(u => u.UsersLecturers)
                                      .Where(u => u.Id == input.Id)
                                      .FirstOrDefaultAsync(cancellationToken);



            user.Id = input.Id.HasValue ? (int)input.Id : 0;
            user.Name = input.Name;
            user.SurName = input.Surname;
            user.Age = input.Age.HasValue ? (int)input.Age : 0;
            user.FacultyId = input.FacultyId.HasValue ? input.FacultyId : null;

            await _contextMethods.UserRepository.UpdateUserAsync(user, cancellationToken);


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
                throw new  NotFoundException("User not found");
            }


            await _contextMethods.UserRepository.SaveChangesAsync(cancellationToken);

            var successResult = ApiResponse<string>.SuccesResult("Course changed successfully");
            return successResult;

        }

        public async Task<ApiResponse<string>> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Faculty)
                .Include(u => u.UsersCourses)
                .Include(u => u.UsersLecturers)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new  NotFoundException("User not found on that Id");
            }

            if (await _contextMethods.UserRepository.DeleteUserAsync(userId, cancellationToken) &&
                    await _contextMethods.UserRepository.DeleteUsersCoursesAsync(userId, cancellationToken) &&
                    await _contextMethods.UserRepository.DeleteUsersLecturers(userId, cancellationToken))
            {
                await _contextMethods.UserRepository.SaveChangesAsync(cancellationToken);
                var successResult = ApiResponse<string>.SuccesResult("User deleted successfully");
                return successResult;
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<ApiResponse<GetDtoWithCount<IEnumerable<TodosDto>>>> GetTodosInfo(TodosDto filter, CancellationToken cancellationToken)
        {
            HttpClient httpClient = new();

            string apiUrl = string.Format(_configuration["ApiUrl"], "todos");
            var response = await httpClient.GetAsync(apiUrl, cancellationToken);
            var data = response.Content.ReadFromJsonAsync<IEnumerable<TodosDto>>().Result;

            if (filter.UserId != null)
                data = data.Where(d => d.UserId == filter.UserId);
            if (filter.TaskId != null)
                data = data.Where(d => d.TaskId == filter.TaskId);
            if (filter.Status.HasValue)
                data = data.Where(d => d.Status == filter.Status);
            if (!filter.Title.IsNullOrEmpty())
                data = data.Where(d => d.Title.Contains(filter.Title));


            return ApiResponse<GetDtoWithCount<IEnumerable<TodosDto>>>.SuccesResult(new GetDtoWithCount<IEnumerable<TodosDto>>
            {
                Data = data,
                Count = data.Count()
            });
        }
    }

}
