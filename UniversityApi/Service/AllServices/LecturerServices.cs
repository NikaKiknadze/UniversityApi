using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.CustomResponses;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repository;
using UniversityApi.Repository.Repositoryes;
using UniversityApi.Service.ServiceAbstracts;

namespace UniversityApi.Service.Services
{
    public class LecturerServices : ILecturerServices
    {
        private readonly UniversistyContext _context;
        private readonly IRepositories _repositories;

        public LecturerServices(IRepositories repositories, UniversistyContext context)
        {
            _context = context;
            _repositories = repositories;
        }


        public async Task<ApiResponse<GetDtosWithCount<List<LecturerGetDto>>>> GetLecturersAsync(LecturerGetFilter filter, CancellationToken cancellationToken)
        {
            var lecturers = await _repositories.LecturerRepository.GetLecturersWithRelatedDataAsync(cancellationToken);

            if (lecturers == null)
            {
                throw new CustomExceptions.NotFoundException("Lecturers not found");
            }

            var filteredLecturers = FilterData(filter, lecturers);

            if(filteredLecturers.Count == 0)
            {
                throw new CustomExceptions.NotFoundException("Lecturers not found");
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

            return ApiResponse<GetDtosWithCount<List<LecturerGetDto>>>.SuccesResult(new GetDtosWithCount<List<LecturerGetDto>>
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

            await _repositories.LecturerRepository.CreateLecturerAsync(lecturer, cancellationToken);
            await _repositories.LecturerRepository.SaveChangesAsync(cancellationToken);


            var lecturerQueryable = await _repositories.LecturerRepository.GetLecturersAsync(cancellationToken);
            var fetchedLecturer = await lecturerQueryable
                                               .Include(l => l.UsersLecturers)
                                                    .ThenInclude(ul => ul.User)
                                               .Include(l => l.CoursesLecturers)
                                                     .ThenInclude(cl => cl.Course)
                                                .FirstOrDefaultAsync(l => l.Id == lecturer.Id);
            if (fetchedLecturer == null)
            {
                throw new CustomExceptions.NotFoundException("Lecturer not found");
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
            var lecturerQueryable = await _repositories.LecturerRepository.GetLecturersAsync(cancellationToken);
            var lecturer = await lecturerQueryable.AsQueryable()
                                              .Include(l => l.UsersLecturers)
                                              .Include(l => l.CoursesLecturers)
                                              .Where(l => l.Id == input.Id)
                                              .FirstOrDefaultAsync();

            lecturer.Id = input.Id.HasValue ? input.Id.Value : 0;
            lecturer.Name = input.Name;
            lecturer.SurName = input.Surname;
            lecturer.Age = input.Id.HasValue ? input.Age.Value : 0;

            if (await _repositories.LecturerRepository.UpdateLecturerAsync(lecturer, cancellationToken))
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
                    throw new CustomExceptions.NotFoundException("Lecturer not found");
                }

                await _repositories.LecturerRepository.SaveChangesAsync(cancellationToken);
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
                throw new CustomExceptions.NotFoundException("Lecturer not fonund on that Id");
            }

            if (await _repositories.LecturerRepository.DeleteLecturerAsync(lecturerId, cancellationToken) &&
                   await _repositories.LecturerRepository.DeleteUsersLecturersAsync(lecturerId, cancellationToken) &&
                   await _repositories.LecturerRepository.DeleteCoursesLecturersAsync(lecturerId, cancellationToken))
            {
                await _repositories.LecturerRepository.SaveChangesAsync(cancellationToken);
                return ApiResponse<string>.SuccesResult("Lecturer deleted successfully");
            }
            else
            {
                throw new Exception();
            }
            

        }
    }
}
