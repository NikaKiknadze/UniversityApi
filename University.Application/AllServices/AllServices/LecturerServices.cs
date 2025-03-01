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
    public class LecturerServices : ILecturerServices
    {
        private readonly UniversityContext _context;
        private readonly ContextMethods _contextMethods;

        public LecturerServices(ContextMethods contextMethods, UniversityContext context)
        {
            _context = context;
            _contextMethods = contextMethods;
        }


        public async Task<ApiResponse<GetDtoWithCount<List<LecturerGetDto>>>> GetLecturersAsync(LecturerGetFilter filter, CancellationToken cancellationToken)
        {
            var lecturers = await _contextMethods.LecturerRepository.GetLecturersWithRelatedDataAsync(cancellationToken);

            if (lecturers == null)
            {
                throw new  NotFoundException("Lecturers not found");
            }

            var filteredLecturers = FilterData(filter, lecturers);

            if(filteredLecturers.Count == 0)
            {
                throw new  NotFoundException("Lecturers not found");
            }

            var lecturerDtos = filteredLecturers.Select(lecturer => new LecturerGetDto
            {
                Id = lecturer.Id,
                Name = lecturer.Name,
                SurName = lecturer.SurName,
                Age = lecturer.Age,
                Users = lecturer.UsersLecturers != null
                                ? lecturer.UsersLecturers.Where(ul => ul.User != null).Select(l => new UserOnlyDto
                                {
                                    Id = l.User.Id,
                                    Name = l.User.Name,
                                    SurName = l.User.SurName,
                                    Age = l.User.Age
                                }).ToList()
                                : new List<UserOnlyDto>(),
                Courses = lecturer.CoursesLecturers != null
                                  ? lecturer.CoursesLecturers.Where(cl => cl.Course != null).Select(l => new CourseOnlyDto
                                  {
                                      Id = l.Course.Id,
                                      CourseName = l.Course.CourseName
                                  }).ToList()
                                  : new List<CourseOnlyDto>()
            })
                .OrderByDescending(l => l.Id)
                .Skip(filter.Offset ?? 0)
                .Take(filter.Limit ?? 10)
                .ToList();

            return ApiResponse<GetDtoWithCount<List<LecturerGetDto>>>.SuccesResult(new GetDtoWithCount<List<LecturerGetDto>>
            {
                Data = lecturerDtos,
                Count = filteredLecturers.Count()
            });

        }

        public List<Lecturer> FilterData(LecturerGetFilter filter, IQueryable<Lecturer> lecturers)
        {
            if(filter.Id != null)
            {
                lecturers = lecturers.Where(l => l.Id == filter.Id);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                lecturers = lecturers.Where(l => l.Name.Contains(filter.Name));
            }
            if (!filter.SurName.IsNullOrEmpty())
            {
                lecturers = lecturers.Where(l => l.SurName.Contains(filter.SurName));
            }
            if(filter.Age != null)
            {
                lecturers = lecturers.Where(l => l.Age == filter.Age);
            }
            if(filter.UserIds != null && filter.UserIds.Any())
            {
                lecturers = lecturers.Where(l => l.UsersLecturers
                                     .Select(l => l.UserId)
                                     .Any(lecturerId => filter.UserIds.Contains(lecturerId)));
            }
            if(filter.CourseIds != null && filter.CourseIds.Any())
            {
                lecturers = lecturers.Where(l => l.CoursesLecturers
                                     .Select(l => l.CourseId)
                                     .Any(lecturerId => filter.CourseIds.Contains(lecturerId)));
            }
            return lecturers.ToList();
        }

        public async Task<ApiResponse<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input, CancellationToken cancellationToken)
        {
            var lecturer = new Lecturer
            {
                Name = input.Name,
                SurName = input.Surname,
                Age = (int)input.Age
            };

            if (!input.CourseIds.IsNullOrEmpty())
            {
                lecturer.CoursesLecturers = new List<CoursesLecturersJoin>();

                foreach (var courseId in input.CourseIds)
                {
                    lecturer.CoursesLecturers.Add(new CoursesLecturersJoin()
                    {
                        CourseId = courseId,
                        LectureId = lecturer.Id
                    });
                }
            }
            else
            {
                throw new Exception();
            }

            if (!input.UserIds.IsNullOrEmpty())
            {
                lecturer.UsersLecturers = new List<UsersLecturersJoin>();

                foreach (var userId in input.UserIds)
                {
                    lecturer.UsersLecturers.Add(new UsersLecturersJoin()
                    {
                        UserId = userId,
                        LecturerId = lecturer.Id
                    });
                }
            }
            else
            {
                throw new Exception();
            }

            await _contextMethods.LecturerRepository.CreateLecturerAsync(lecturer, cancellationToken);
            await _contextMethods.LecturerRepository.SaveChangesAsync(cancellationToken);


            var lecturerQueryable = await _contextMethods.LecturerRepository.GetLecturersAsync(cancellationToken);
            var fetchedLecturer = await lecturerQueryable
                                               .Include(l => l.UsersLecturers)
                                                    .ThenInclude(ul => ul.User)
                                               .Include(l => l.CoursesLecturers)
                                                     .ThenInclude(cl => cl.Course)
                                                .FirstOrDefaultAsync(l => l.Id == lecturer.Id);
            if (fetchedLecturer == null)
            {
                throw new  NotFoundException("Lecturer not found");
            }

            var lecturerDto = new LecturerGetDto
            {
                Id = fetchedLecturer.Id,
                Name = fetchedLecturer.Name,
                SurName = fetchedLecturer.SurName,
                Age = fetchedLecturer.Age,
                Users = fetchedLecturer.UsersLecturers != null
                                ? fetchedLecturer.UsersLecturers.Where(ul => ul.UserId != null).Select(l => new UserOnlyDto
                                {
                                    Id = l.User.Id,
                                    Name = l.User.Name,
                                    SurName = l.User.SurName,
                                    Age = l.User.Age
                                }).ToList()
                                : new List<UserOnlyDto>(),
                Courses = fetchedLecturer.CoursesLecturers != null
                                  ? fetchedLecturer.CoursesLecturers.Where(cl => cl.CourseId != null).Select(l => new CourseOnlyDto
                                  {
                                      Id = l.Course.Id,
                                      CourseName = l.Course.CourseName
                                  }).ToList()
                                  : new List<CourseOnlyDto>()
            };

            return ApiResponse<LecturerGetDto>.SuccesResult(lecturerDto);
        }

        public async Task<ApiResponse<string>> UpdateLecturerAsync(LecturerPutDto input, CancellationToken cancellationToken)
        {
            var lecturerQueryable = await _contextMethods.LecturerRepository.GetLecturersAsync(cancellationToken);
            var lecturer = await lecturerQueryable.AsQueryable()
                                              .Include(l => l.UsersLecturers)
                                              .Include(l => l.CoursesLecturers)
                                              .Where(l => l.Id == input.Id)
                                              .FirstOrDefaultAsync();

            lecturer.Id = input.Id.HasValue ? input.Id.Value : 0;
            lecturer.Name = input.Name;
            lecturer.SurName = input.Surname;
            lecturer.Age = input.Id.HasValue ? input.Age.Value : 0;

            if (await _contextMethods.LecturerRepository.UpdateLecturerAsync(lecturer, cancellationToken))
            {
                if (!input.UserIds.IsNullOrEmpty())
                {
                    lecturer.UsersLecturers.Clear();
                    foreach (var userId in input.UserIds)
                    {
                        lecturer.UsersLecturers.Add(new UsersLecturersJoin()
                        {
                            UserId = userId,
                            LecturerId = lecturer.Id
                        });
                    }
                }

                if (!input.CourseIds.IsNullOrEmpty())
                {
                    lecturer.CoursesLecturers.Clear();
                    foreach (var courseId in input.CourseIds)
                    {
                        lecturer.CoursesLecturers.Add(new CoursesLecturersJoin()
                        {
                            CourseId = courseId,
                            LectureId = lecturer.Id
                        });
                    }
                }

                if (lecturer == null)
                {
                    throw new  NotFoundException("Lecturer not found");
                }

                await _contextMethods.LecturerRepository.SaveChangesAsync(cancellationToken);
                return ApiResponse<string>.SuccesResult("Lecturer changed successfully");
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<ApiResponse<string>> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken)
        {
            var lecturer = await _context.Lecturers
                                   .Include(ul => ul.UsersLecturers)
                                        .ThenInclude(u => u.User)
                                    .Include(cl => cl.CoursesLecturers)
                                        .ThenInclude(c => c.Course)
                                    .FirstOrDefaultAsync(l => l.Id == lecturerId, cancellationToken);
            
            if(lecturer == null)
            {
                throw new  NotFoundException("Lecturer not fonund on that Id");
            }

            if (await _contextMethods.LecturerRepository.DeleteLecturerAsync(lecturerId, cancellationToken) &&
                   await _contextMethods.LecturerRepository.DeleteUsersLecturersAsync(lecturerId, cancellationToken) &&
                   await _contextMethods.LecturerRepository.DeleteCoursesLecturersAsync(lecturerId, cancellationToken))
            {
                await _contextMethods.LecturerRepository.SaveChangesAsync(cancellationToken);
                return ApiResponse<string>.SuccesResult("Lecturer deleted successfully");
            }
            else
            {
                throw new Exception();
            }
            

        }
    }
}
