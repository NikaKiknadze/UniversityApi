using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.CustomResponses;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repositories;

namespace UniversityApi.Services
{
    public class LecturerServices
    {
        private readonly UniversistyContext _context;
        private readonly LecturerRepository _lecturerRepository
            ;

        public LecturerServices(LecturerRepository lecturerRepository, UniversistyContext context)
        {
            _context = context;
            _lecturerRepository = lecturerRepository;
        }


        public async Task<ApiResponse<List<LecturerGetDto>>> GetLecturersAsync()
        {
            try
            {
                var lecturers = await _lecturerRepository.GetLecturersWithRelatedDataAsync();

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

                await _lecturerRepository.CreateLecturerAsync(lecturer);
                await _lecturerRepository.SaveChangesAsync();


                var lecturerQueryable = await _lecturerRepository.GetLecturersAsync();
                var fetchedLecturer = await lecturerQueryable
                                                   .Include(l => l.UsersLecturers)
                                                        .ThenInclude(ul => ul.User)
                                                   .Include(l => l.CoursesLecturers)
                                                         .ThenInclude(cl => cl.Course)
                                                    .FirstOrDefaultAsync(l => l.Id == lecturer.Id);
                if(fetchedLecturer == null)
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
                var lecturerQueryable = await _lecturerRepository.GetLecturersAsync();
                var lecturer = await lecturerQueryable.AsQueryable()
                                                  .Include(l => l.UsersLecturers)
                                                  .Include(l => l.CoursesLecturers)
                                                  .Where(l => l.Id == input.Id)
                                                  .FirstOrDefaultAsync();

                lecturer.Id = input.Id.HasValue ? (int)input.Id.Value : 0;
                lecturer.Name = input.Name;
                lecturer.SurName = input.Surname;
                lecturer.Age = input.Id.HasValue ? (int)input.Age.Value : 0;

                if (await _lecturerRepository.UpdateLecturerAsync(lecturer))
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

                    if(lecturer == null)
                    {
                        return new ApiResponse<bool>(false, "Lecturer not found", false);
                    }

                    await _lecturerRepository.SaveChangesAsync();
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
                if (await _lecturerRepository.DeleteLecturerAsync(lecturerId) &&
                   await _lecturerRepository.DeleteUsersLecturersAsync(lecturerId) &&
                   await _lecturerRepository.DeleteCoursesLecturersAsync(lecturerId))
                {
                    await _lecturerRepository.SaveChangesAsync();
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
