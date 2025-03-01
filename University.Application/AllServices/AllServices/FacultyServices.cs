using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using University.Application.AllServices.ServiceAbstracts;
using University.Data.ContextMethodsDirectory;
using University.Data.Data;
using University.Data.Data.Entities;
using University.Data.Data.EntityGenericMethods;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;
using University.Domain.Models;

namespace University.Application.AllServices.AllServices
{
    public class FacultyServices : IFacultyServices
    {
        public readonly IEntityGenericMethods<Faculty> _facultyRepository;
        public FacultyServices(IEntityGenericMethods<Faculty> facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }

        

        public async Task<ApiResponse<GetDtoWithCount<List<FacultyGetDto>>>> GetFacultiesAsync(FacultyGetFilter filter,CancellationToken cancellationToken)
        {
            var faculties = await _facultyRepository.All.GetFacultiesWithRelatedDataAsync(cancellationToken);

            if(faculties == null)
            {
                throw new  NotFoundException("Faculties not found");
            }

            var filteredFaculties = FilterData(filter, faculties);

            if(filteredFaculties == null)
            {
                throw new  NotFoundException("Faculty not found");
            }

            var facultyDtos = filteredFaculties.Select(f => new FacultyGetDto
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
            })
                .OrderByDescending(f => f.Id)
                .Skip(filter.Offset ?? 0)
                .Take(filter.Limit ?? 10)
                .ToList();

            return ApiResponse<GetDtoWithCount<List<FacultyGetDto>>>.SuccesResult(new GetDtoWithCount<List<FacultyGetDto>>
            {
                Data = facultyDtos,
                Count = filteredFaculties.Count()
            });
        }

        public List<Faculty> FilterData(FacultyGetFilter filter, IQueryable<Faculty> faculties)
        {
            if (filter.Id != null)
            {
                faculties = faculties.Where(f => f.Id == filter.Id);
            }
            if (!filter.FacultyName.IsNullOrEmpty())
            {
                faculties = faculties.Where(f => f.FacultyName.Contains(filter.FacultyName));
            }
            if (filter.UserIds != null && filter.UserIds.Any())
            {
                faculties = faculties.Where(f => f.Users
                                 .Select(f => f.Id)
                                 .Any(facultyId => filter.UserIds.Contains(facultyId)));
            }
            if (filter.CourseIds != null && filter.CourseIds.Any())
            {
                faculties = faculties.Where(f => f.Courses
                                 .Select(f => f.Id)
                                 .Any(facultyId => filter.CourseIds.Contains(facultyId)));
            }
            return faculties.ToList();
        }

        public async Task<ApiResponse<FacultyGetDto>> CreateFacultyAsync(FacultyPostDto input, CancellationToken cancellationToken)
        {
            var faculty = new Faculty
            {
                FacultyName = input.FacultyName
            };


            await _facultyRepository.All.CreateFacultyAsync(faculty, cancellationToken);



            if (!input.UserIds.IsNullOrEmpty())
            {
                faculty.Users = new List<User>();

                foreach (var userId in input.UserIds)
                {
                    var userQueryable = await _facultyRepository.All.GetUsersAsync(cancellationToken);
                    var user = await userQueryable.AsQueryable()
                                              .Include(u => u.UsersCourses)
                                              .Include(u => u.UsersLecturers)
                                              .Where(u => u.Id == userId)
                                              .FirstOrDefaultAsync(cancellationToken);
                    if (user != null)
                    {
                        user.FacultyId = faculty.Id;
                        faculty.Users.Add(user);
                    }
                }
            }
            else
            {
                throw new Exception();
            }

            if (!input.CourseIds.IsNullOrEmpty())
            {
                faculty.Courses = new List<Course?>();

                foreach (var courseId in input.CourseIds)
                {
                    var course = await _facultyRepository.All.GetCourseByIdAsync(courseId, cancellationToken);

                    if (course != null)
                    {
                        course.FacultyId = faculty.Id;
                        faculty.Courses.Add(course);

                    }
                }
            }
            else
            {
                throw new Exception();
            }

            await ContextMethods.UserRepository.SaveChangesAsync(cancellationToken);
            await ContextMethods.FacultyRepository.SaveChangesAsync(cancellationToken);

            var facultyQueryable = await ContextMethods.FacultyRepository.GetFacultiesAsync(cancellationToken);
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
                throw new  NotFoundException("Faculty not found");
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

            var successResult = ApiResponse<FacultyGetDto>.SuccesResult(facultyDto);
            return successResult;
        }

        public async Task<ApiResponse<string>> UpdateFacultyAsync(FacultyPutDto input, CancellationToken cancellationToken)
        {
            var facultyQueryable = await ContextMethods.FacultyRepository.GetFacultiesAsync(cancellationToken);
            var faculty = await facultyQueryable.AsQueryable()
                .Include(f => f.Users)
                .Include(f => f.Courses)
                .Where(f => f.Id == input.Id)
                .FirstOrDefaultAsync();



            faculty.Id = input.Id.HasValue ? input.Id.Value : 0;
            faculty.FacultyName = input.FacultyName;

            if (await ContextMethods.FacultyRepository.UpdateFacultyAsync(faculty, cancellationToken))
            {
                if (faculty == null)
                {
                    throw new  NotFoundException("Faculty not found");
                }

                if (!input.UserIds.IsNullOrEmpty())
                {
                    faculty.Users.Clear();
                    foreach (var userId in input.UserIds)
                    {
                        var userQueryable = await ContextMethods.UserRepository.GetUsersAsync(cancellationToken);
                        var user = await userQueryable.AsQueryable()
                                                  .Include(u => u.UsersCourses)
                                                  .Include(u => u.UsersLecturers)
                                                  .Where(u => u.Id == input.Id)
                                                  .FirstOrDefaultAsync(cancellationToken);

                        faculty.Users.Add(user);
                    }
                }
                else
                {
                    throw new Exception();
                }

                if (!input.CourseIds.IsNullOrEmpty())
                {
                    faculty.Courses.Clear();
                    foreach (var courseId in input.CourseIds)
                    {
                        var course = await ContextMethods.CourseRepository.GetCourseByIdAsync(courseId, cancellationToken);
                        faculty.Courses.Add(course);
                    }
                }
                else
                {
                    throw new Exception();
                }

                await ContextMethods.FacultyRepository.SaveChangesAsync(cancellationToken);
                var successResult = ApiResponse<string>.SuccesResult("Faculty updated successfully");
                return successResult;
            }
            else
            {
                throw new Exception();
            }

        }

        public async Task<ApiResponse<string>> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken)
        {
            var faculty = await Context.Faculty
                                 .Include(f => f.Users)
                                 .Include(f => f.Courses)
                                 .FirstOrDefaultAsync(f => f.Id == facultyId, cancellationToken);
            if (faculty == null)
            {
                throw new  NotFoundException("Faculty not found on that Id");
            }

            if (await ContextMethods.FacultyRepository.DeleteFacultyAsync(facultyId, cancellationToken))
            {
                await ContextMethods.FacultyRepository.SaveChangesAsync(cancellationToken);
                var successResult = ApiResponse<string>.SuccesResult("Faculty deleted successfully");
                return successResult;
            }
            else
            {
                throw new Exception();
            }
            
        }
    }
}
