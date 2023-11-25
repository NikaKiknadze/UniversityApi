﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.CustomResponses;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repositories;

namespace UniversityApi.Services
{
    public class CourseServices
    {
        public readonly UniversistyContext _context;
        public readonly CourseRepository _courseRepository;

        public CourseServices(UniversistyContext context, CourseRepository repository)
        {
            _context = context;
            _courseRepository = repository;
        }


        public ApiResponse<List<CourseGetDto>> GetCourses()
        {
            try
            {
                var courses = _courseRepository.GetCoursesWithRelatedData();

                var courseDtos = courses.Select(course => new CourseGetDto
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    Faculty = course.Faculty != null
                                  ? new FacultyOnlyDto
                                  {
                                      Id = course.Faculty.Id,
                                      FacultyName = course.Faculty.FacultyName
                                  }
                                  : null,
                    Lecturers = course.CoursesLecturers != null
                                       ? course.CoursesLecturers.Where(ul => ul.Lecturer != null).Select(c => new LecturerOnlyDto
                                       {
                                           Id = c.Lecturer.Id,
                                           Name = c.Lecturer.Name,
                                           SurName = c.Lecturer.SurName,
                                           Age = c.Lecturer.Age
                                       }).ToList()
                                       : new List<LecturerOnlyDto>(),
                    Users = course.UsersCourses != null
                                   ? course.UsersCourses.Where(uc => uc.User != null).Select(c => new UserOnlyDto
                                   {
                                       Id = c.User.Id,
                                       Name = c.User.Name,
                                       SurName = c.User.SurName,
                                       Age = c.User.Age
                                   }).ToList()
                                   : new List<UserOnlyDto>()
                }).ToList();

                return new ApiResponse<List<CourseGetDto>>(true, "Courses fetched successfully", courseDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CourseGetDto>>(false, $"Error : {ex.Message}", null);
            }
        }

        public ApiResponse<CourseGetDto> CreateCourse(CoursePostDto input)
        {
            try
            {
                var course = new Course
                {
                    CourseName = input.CourseName,
                    UsersCourses = new List<UsersCoursesJoin>(),
                    CoursesLecturers = new List<CoursesLecturersJoin>()
                };

                if (!input.UserIds.IsNullOrEmpty())
                {
                    course.UsersCourses = new List<UsersCoursesJoin>();

                    foreach (var userId in input.UserIds)
                    {
                        course.UsersCourses.Add(new UsersCoursesJoin()
                        {
                            UserId = userId,
                            CourseId = course.Id
                        });
                    }
                }

                if (!input.LecturerIds.IsNullOrEmpty())
                {
                    course.CoursesLecturers = new List<CoursesLecturersJoin>();

                    foreach (var lecturerId in input.LecturerIds)
                    {
                        course.CoursesLecturers.Add(new CoursesLecturersJoin()
                        {
                            LectureId = lecturerId,
                            CourseId = course.Id
                        });
                    }
                }

                _courseRepository.CreateCourse(course);
                _courseRepository.SaveChanges();

                var courseDto = new CourseGetDto
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    Faculty = course.Faculty != null
                                  ? new FacultyOnlyDto
                                  {
                                      Id = course.Faculty.Id,
                                      FacultyName = course.Faculty.FacultyName
                                  }
                                  : null,
                    Lecturers = course.CoursesLecturers != null
                                       ? course.CoursesLecturers.Where(ul => ul.Lecturer != null).Select(c => new LecturerOnlyDto
                                       {
                                           Id = c.Lecturer.Id,
                                           Name = c.Lecturer.Name,
                                           SurName = c.Lecturer.SurName,
                                           Age = c.Lecturer.Age
                                       }).ToList()
                                       : new List<LecturerOnlyDto>(),
                    Users = course.UsersCourses != null
                                   ? course.UsersCourses.Where(uc => uc.User != null).Select(c => new UserOnlyDto
                                   {
                                       Id = c.User.Id,
                                       Name = c.User.Name,
                                       SurName = c.User.SurName,
                                       Age = c.User.Age
                                   }).ToList()
                                   : new List<UserOnlyDto>()
                };

                return new ApiResponse<CourseGetDto>(true, "Course created successfully", courseDto);
                
            }
            catch (Exception ex)
            {
                return new ApiResponse<CourseGetDto>(false, $"Error: {ex.Message}", null);
            }
            
        }

        public ApiResponse<bool> UpdateCourse(CoursePutDto input)
        {
            try
            {
                var course = _courseRepository.GetCourses()
                                              .Include(c => c.UsersCourses)
                                                    .ThenInclude(uc => uc.User)
                                              .Include(c => c.CoursesLecturers)
                                                    .ThenInclude(cl => cl.Lecturer)
                                              .Where(c => c.Id == input.Id)
                                              .FirstOrDefault();
                course.Id = input.Id.HasValue ? (int)input.Id : 0;
                course.CourseName = input.CourseName;
                course.FacultyId = input.FacultyId.HasValue ? (int)input.FacultyId : null;

                if (_courseRepository.UpdateCourse(course))
                {
                    if (!input.UserIds.IsNullOrEmpty())
                    {
                        foreach (var userId in input.UserIds)
                        {
                            course.UsersCourses.Clear();
                            course.UsersCourses.Add(new UsersCoursesJoin()
                            {
                                UserId = userId,
                                CourseId = course.Id
                            });
                        }
                        return new ApiResponse<bool>(true, "UsersCourses updated", true);
                    }

                    if (!input.LecturerIds.IsNullOrEmpty())
                    {
                        foreach (var lecturerId in input.LecturerIds)
                        {
                            course.CoursesLecturers.Clear();
                            course.CoursesLecturers.Add(new CoursesLecturersJoin()
                            {
                                LectureId = lecturerId,
                                CourseId = course.Id
                            });
                        }
                        return new ApiResponse<bool>(true, "CoursesLecturers updated", true);
                    }
                    _courseRepository.SaveChanges();
                    return new ApiResponse<bool>(true, "Course changed Successfully", true);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
            return new ApiResponse<bool>(false, "Unexpected error occurred", false);

        }

        public ApiResponse<bool> DeleteCourse(int courseId)
        {
            try
            {
                if (_courseRepository.DeleteCourse(courseId) &&
                       _courseRepository.DeleteUsersCourses(courseId) &&
                       _courseRepository.DeleteCourseLecturers(courseId))
                    {
                        _courseRepository.SaveChanges();
                        return new ApiResponse<bool>(true, "Course deleted successfully", true);
                    }
                return new ApiResponse<bool>(false, "Failed to delete Course", false);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(false, $"Error: {ex.Message}", false);
            }
            
        }
    }
}
