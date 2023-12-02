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


        public async Task<ApiResponse<List<LecturerGetDto>>> GetLecturersAsync()
        {
            try
            {
                var lecturers = await _repositories.LecturerRepository.GetLecturersWithRelatedDataAsync();

                var lecturerDtos = lecturers.Select(lecturer => new LecturerGetDto
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
                }).ToList();

                return new ApiResponse<List<LecturerGetDto>>(true, "Lecturers fetched successfully", lecturerDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<LecturerGetDto>>(false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input)
        {
            try
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

                await _repositories.LecturerRepository.CreateLecturerAsync(lecturer);
                await _repositories.LecturerRepository.SaveChangesAsync();


                var lecturerQueryable = await _repositories.LecturerRepository.GetLecturersAsync();
                var fetchedLecturer = await lecturerQueryable
                                                   .Include(l => l.UsersLecturers)
                                                        .ThenInclude(ul => ul.User)
                                                   .Include(l => l.CoursesLecturers)
                                                         .ThenInclude(cl => cl.Course)
                                                    .FirstOrDefaultAsync(l => l.Id == lecturer.Id);
                if (fetchedLecturer == null)
                {
                    return new ApiResponse<LecturerGetDto>(false, "Lecturer not found", null);
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

                return new ApiResponse<LecturerGetDto>(true, "Lecturer created successfully", lecturerDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<LecturerGetDto>(false, $"Error: {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<bool>> UpdateLecturerAsync(LecturerPutDto input)
        {
            try
            {
                var lecturerQueryable = await _repositories.LecturerRepository.GetLecturersAsync();
                var lecturer = await lecturerQueryable.AsQueryable()
                                                  .Include(l => l.UsersLecturers)
                                                  .Include(l => l.CoursesLecturers)
                                                  .Where(l => l.Id == input.Id)
                                                  .FirstOrDefaultAsync();

                lecturer.Id = input.Id.HasValue ? input.Id.Value : 0;
                lecturer.Name = input.Name;
                lecturer.SurName = input.Surname;
                lecturer.Age = input.Id.HasValue ? input.Age.Value : 0;

                if (await _repositories.LecturerRepository.UpdateLecturerAsync(lecturer))
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
                        return new ApiResponse<bool>(false, "Lecturer not found", false);
                    }

                    await _repositories.LecturerRepository.SaveChangesAsync();
                    return new ApiResponse<bool>(true, "Lecturer changed successfully", true);
                }
                return new ApiResponse<bool>(false, "Failed to update Lecturer", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }

        public async Task<ApiResponse<bool>> DeleteLecturerAsync(int lecturerId)
        {
            try
            {
                if (await _repositories.LecturerRepository.DeleteLecturerAsync(lecturerId) &&
                   await _repositories.LecturerRepository.DeleteUsersLecturersAsync(lecturerId) &&
                   await _repositories.LecturerRepository.DeleteCoursesLecturersAsync(lecturerId))
                {
                    await _repositories.LecturerRepository.SaveChangesAsync();
                    return new ApiResponse<bool>(true, "Lecturer deleted successfully", true);
                }
                return new ApiResponse<bool>(false, "Failed to delete Lecturer", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }
    }
}
