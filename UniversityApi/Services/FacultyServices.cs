using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repositories;

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

        public List<FacultyGetDto> GetFacultyById(int facultyId)
        {
            var faculty = _facultyRepository.GetFaculties()
                                            .Include(f => f.Users)
                                            .Include(f => f.Courses)
                                            .Where(f => f.Id == facultyId)
                                            .Select(f => new FacultyGetDto
                                            {
                                                Id = f.Id,
                                                FacultyName = f.FacultyName,
                                                UserIds = f.Users != null 
                                                          ? f.Users.Select(f => f.Id).ToList()
                                                          : new List<int>(),
                                                CourseIds = f.Courses != null
                                                            ? f.Courses.Select(f => f.Id).ToList() 
                                                            : new List<int>()
                                            }).ToList();
            return faculty;
        }

        public List<FacultyGetDto> GetFaculties()
        {
            var faculties = _facultyRepository.GetFaculties()
                                              .Include(f => f.Users)
                                              .Include(f => f.Courses)
                                              .ToList();
            var facultyDtos = faculties.Select(faculty => new FacultyGetDto
            {
                Id = faculty.Id,
                FacultyName = faculty.FacultyName,
                UserIds = faculty.Users?.Select(f => f.Id).ToList() ?? new List<int>(),
                CourseIds = faculty.Courses?.Select(f => f.Id).ToList() ?? new List<int>()
            }).ToList();

            return facultyDtos;
                                              
        }

        public FacultyGetDto CreateFaculty(FacultyPostDto input)
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
                foreach(var userId in input.UserIds)
                {
                    var user = _userRepository.GetUserById(userId);
                    if(user != null)
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

                    if(course != null)
                    {
                        course.FacultyId = faculty.Id;
                        faculty.Courses.Add(course);
                        
                    }
                    //_courseRepository.SaveChanges();
                }
            }

            _userRepository.SaveChanges();
            _facultyRepository.SaveChanges();



            return new FacultyGetDto
            {
                Id = faculty.Id,
                FacultyName = faculty.FacultyName,
                UserIds = faculty.Users.Select(f => f.Id).ToList(),
                CourseIds = faculty.Courses.Select(f => f.Id).ToList()
            };


        }

        public bool UpdateFaculty(FacultyPutDto input)
        {
            var faculty = _facultyRepository.GetFaculties()
                                            .Include(f => f.Users)
                                            .Include(f => f.Courses)
                                            .Where(f => f.Id == input.Id)
                                            .FirstOrDefault();
            faculty.Id = input.Id.HasValue ? (int)input.Id.Value : 0;
            faculty.FacultyName = input.FacultyName;

            if (_facultyRepository.UpdateFaculty(faculty))
            {
                if (!input.UserIds.IsNullOrEmpty())
                {
                    faculty.Users.Clear();
                    foreach(var userId in input.UserIds)
                    {
                        var user = _userRepository.GetUserById(userId);
                        faculty.Users.Add(user);
                    }
                }
                _facultyRepository.SaveChanges();

                if (!input.CourseIds.IsNullOrEmpty())
                {
                    faculty.Courses.Clear();
                    foreach(var courseId in input.CourseIds)
                    {
                        var course = _courseRepository.GetCourseById(courseId);
                        faculty.Courses.Add(course);
                    }
                }

                _facultyRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteFaculty(FacultyDeleteDto input)
        {
            if (_facultyRepository.DeleteFaculty(input.Id))
            {
                _facultyRepository.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
