using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public List<UserGetDto> GetUserById(int userId)
        {
            var user = _userRepository.GetUsers()
                                      .Include(u => u.UsersLecturers)
                                      .Include(u => u.UsersCourses)
                                      .Where(u => u.Id == userId)
                                      .Select(u => new UserGetDto
                                      {
                                          Id = u.Id,
                                          Name = u.Name,
                                          SurName = u.SurName,
                                          Age = u.Age,
                                          Faculty = u.Faculty != null
                                                    ? new FacultyGetDto
                                          {
                                              Id = (int)u.Id
                                          }
                                          : null,
                                          CourseIds = u.UsersCourses != null
                                                      ? u.UsersCourses.Select(u => u.CourseId).ToList()
                                                      : new List<int>(),
                                          LecturerIds = u.UsersLecturers != null
                                                        ? u.UsersLecturers.Select(u => u.LecturerId).ToList()
                                                        : new List<int>()
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
                ? new FacultyGetDto
                {
                    Id = (int)user.Faculty.Id,
                    FacultyName = (string)user.Faculty.FacultyName
                } : null,
                CourseIds = user.UsersCourses?.Select(u => u.CourseId).ToList() ?? new List<int>(),
                LecturerIds = user.UsersLecturers?.Select(u => u.LecturerId).ToList() ?? new List<int>()
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

            if(!input.CourseIds.IsNullOrEmpty())
            {
                user.UsersCourses = new List<UsersCoursesJoin>();

                foreach(var courseId in input.CourseIds)
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
                Faculty = new FacultyGetDto
                {
                    Id = user.FacultyId
                },
                CourseIds = user.UsersCourses.Select(u => u.CourseId).ToList(),
                LecturerIds = user.UsersLecturers.Select(u => u.LecturerId).ToList()
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


            if(_userRepository.UpdateUser(user))
            {
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
                _userRepository.SaveChanges();
                return true;
            }

            return false;

        }

        public bool DeleteUser(UserDeleteDto input)
        {
            if (_userRepository.DeleteUser(input.Id) &&
                _userRepository.DeleteUsersCourses(input.Id) &&
                _userRepository.DeleteUsersLecturers(input.Id))
            {
                _userRepository.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
