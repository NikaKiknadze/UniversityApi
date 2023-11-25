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
        /*
        public List<LecturerGetDto> GetLecturers()
        {
            var lecturers = _lecturerRepository.GetLecturersWithRelatedData();

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

            return lecturerDtos;
        }

        public LecturerGetDto CreateLecturer(LecturerPostDto input)
        {
            var lecturer = new Lecturer
            {
                Name = input.Name,
                SurName = input.Surname,
                Age = (int)input.Age,
                UsersLecturers = new List<UsersLecturersJoin>(),
                CoursesLecturers = new List<CoursesLecturersJoin>()
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

            _lecturerRepository.CreateLecturer(lecturer);
            _lecturerRepository.SaveChanges();

            return new LecturerGetDto
            {
                Id = lecturer.Id,
                Name = lecturer.Name,
                SurName = lecturer.SurName,
                Age = lecturer.Age,
                Users = lecturer.UsersLecturers != null
                                 ? lecturer.UsersLecturers.Where(ul => ul.UserId != null).Select(l => new UserOnlyDto
                                 {
                                     Id = l.User.Id,
                                     Name = l.User.Name,
                                     SurName = l.User.SurName,
                                     Age = l.User.Age
                                 }).ToList()
                : new List<UserOnlyDto>(),
                Courses = lecturer.CoursesLecturers != null
                                   ? lecturer.CoursesLecturers.Where(cl => cl.CourseId != null).Select(l => new CourseOnlyDto
                                   {
                                       Id = l.Course.Id,
                                       CourseName = l.Course.CourseName
                                   }).ToList()
                : new List<CourseOnlyDto>()
            };
        }

        public bool UpdateLecturer(LecturerPutDto input)
        {
            var lecturer = _lecturerRepository.GetLecturers()
                                              .Include(l => l.UsersLecturers)
                                              .Include(l => l.CoursesLecturers)
                                              .Where(l => l.Id == input.Id)
                                              .FirstOrDefault();

            lecturer.Id = input.Id.HasValue ? (int)input.Id.Value : 0;
            lecturer.Name = input.Name;
            lecturer.SurName = input.Surname;
            lecturer.Age = input.Id.HasValue ? (int)input.Age.Value : 0;

            if (_lecturerRepository.UpdateLecturer(lecturer))
            {
                if (!input.UserIds.IsNullOrEmpty())
                {
                    foreach (var userId in input.UserIds)
                    {
                        lecturer.UsersLecturers.Clear();
                        lecturer.UsersLecturers.Add(new UsersLecturersJoin()
                        {
                            UserId = userId,
                            LecturerId = lecturer.Id
                        });
                    }
                }

                if (!input.CourseIds.IsNullOrEmpty())
                {
                    foreach (var courseId in input.CourseIds)
                    {
                        lecturer.CoursesLecturers.Clear();
                        lecturer.CoursesLecturers.Add(new CoursesLecturersJoin()
                        {
                            CourseId = courseId,
                            LectureId = lecturer.Id
                        });
                    }
                }
                _lecturerRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteLecturer(int lecturerId)
        {
            if (_lecturerRepository.DeleteLecturer(lecturerId) &&
               _lecturerRepository.DeleteUsersLecturers(lecturerId) &&
               _lecturerRepository.DeleteCoursesLecturers(lecturerId))
            {
                _lecturerRepository.SaveChanges();
                return true;
            }
            return false;
        }

        */

        public ApiResponse<List<LecturerGetDto>> GetLecturers()
        {
            try
            {
                var lecturers = _lecturerRepository.GetLecturersWithRelatedData();

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

        public ApiResponse<LecturerGetDto> CreateLecturer(LecturerPostDto input)
        {
            try
            {
                var lecturer = new Lecturer
                {
                    Name = input.Name,
                    SurName = input.Surname,
                    Age = (int)input.Age,
                    UsersLecturers = new List<UsersLecturersJoin>(),
                    CoursesLecturers = new List<CoursesLecturersJoin>()
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

                _lecturerRepository.CreateLecturer(lecturer);
                _lecturerRepository.SaveChanges();

                var lecturerDto = new LecturerGetDto
                {
                    Id = lecturer.Id,
                    Name = lecturer.Name,
                    SurName = lecturer.SurName,
                    Age = lecturer.Age,
                    Users = lecturer.UsersLecturers != null
                                    ? lecturer.UsersLecturers.Where(ul => ul.UserId != null).Select(l => new UserOnlyDto
                                    {
                                        Id = l.User.Id,
                                        Name = l.User.Name,
                                        SurName = l.User.SurName,
                                        Age = l.User.Age
                                    }).ToList()
                                    : new List<UserOnlyDto>(),
                    Courses = lecturer.CoursesLecturers != null
                                      ? lecturer.CoursesLecturers.Where(cl => cl.CourseId != null).Select(l => new CourseOnlyDto
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

        public ApiResponse<bool> UpdateLecturer(LecturerPutDto input)
        {
            try
            {
                var lecturer = _lecturerRepository.GetLecturers()
                                                  .Include(l => l.UsersLecturers)
                                                  .Include(l => l.CoursesLecturers)
                                                  .Where(l => l.Id == input.Id)
                                                  .FirstOrDefault();

                lecturer.Id = input.Id.HasValue ? (int)input.Id.Value : 0;
                lecturer.Name = input.Name;
                lecturer.SurName = input.Surname;
                lecturer.Age = input.Id.HasValue ? (int)input.Age.Value : 0;

                if (_lecturerRepository.UpdateLecturer(lecturer))
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
                    _lecturerRepository.SaveChanges();
                    return new ApiResponse<bool>(true, "Lecturer changed successfully", true);
                }
                return new ApiResponse<bool>(false, "Failed to update Lecturer", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
        }

        public ApiResponse<bool> DeleteLecturer(int lecturerId)
        {
            try
            {
                if (_lecturerRepository.DeleteLecturer(lecturerId) &&
                   _lecturerRepository.DeleteUsersLecturers(lecturerId) &&
                   _lecturerRepository.DeleteCoursesLecturers(lecturerId))
                {
                    _lecturerRepository.SaveChanges();
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
