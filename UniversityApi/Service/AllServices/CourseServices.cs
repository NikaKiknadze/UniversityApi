﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApi.CustomResponses;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Entities;
using UniversityApi.Repository;
using UniversityApi.Service.ServiceAbstracts;

namespace UniversityApi.Service.Services
{
    public class CourseServices : ICourseServices
    {
        private readonly UniversistyContext _context;
        private readonly IRepositories _repositories;

        public CourseServices(UniversistyContext context, IRepositories repository)
        {
            _context = context;
            _repositories = repository;
        }

        public async Task<ApiResponse<List<CourseGetDto>>> GetCoursesAsync()
        {
            var courses = await _repositories.CourseRepository.GetCoursesWithRelatedDataAsync();

            if (courses == null || !courses.Any())
            {
                throw new CustomExceptions.NoContentException("No courses found.");
            }

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

            var successResult = ApiResponse<List<CourseGetDto>>.SuccesResult(courseDtos);
            return successResult;
        }

        public async Task<ApiResponse<CourseGetDto>> CreateCourseAsync(CoursePostDto input)
        {
            var course = new Course
            {
                CourseName = input.CourseName,
                FacultyId = input.FacultyId
            };

            if (input.CourseName == null)
            {
                throw new CustomExceptions.BadRequestException("Course Name is null");
            }

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

            await _repositories.CourseRepository.CreateCourseAsync(course);
            await _repositories.CourseRepository.SaveChangesAsync();

            var courseQueryable = await _repositories.CourseRepository.GetCoursesAsync();
            var fetchedCourse = await courseQueryable
                                            .Include(c => c.Faculty)
                                            .Include(c => c.UsersCourses)
                                                    .ThenInclude(uc => uc.User)
                                            .Include(c => c.CoursesLecturers)
                                                    .ThenInclude(ul => ul.Lecturer)
                                            .FirstOrDefaultAsync(c => c.Id == course.Id);

            if (fetchedCourse == null)
            {
                throw new CustomExceptions.NoContentException("Course not found");
            }

            var courseDto = new CourseGetDto
            {
                Id = fetchedCourse.Id,
                CourseName = fetchedCourse.CourseName,
                Faculty = fetchedCourse.Faculty != null
                              ? new FacultyOnlyDto
                              {
                                  Id = fetchedCourse.Faculty.Id,
                                  FacultyName = fetchedCourse.Faculty.FacultyName
                              }
                              : null,
                Lecturers = fetchedCourse.CoursesLecturers != null
                                   ? fetchedCourse.CoursesLecturers.Where(ul => ul.Lecturer != null).Select(c => new LecturerOnlyDto
                                   {
                                       Id = c.Lecturer.Id,
                                       Name = c.Lecturer.Name,
                                       SurName = c.Lecturer.SurName,
                                       Age = c.Lecturer.Age
                                   }).ToList()
                                   : new List<LecturerOnlyDto>(),
                Users = fetchedCourse.UsersCourses != null
                               ? fetchedCourse.UsersCourses.Where(uc => uc.User != null).Select(c => new UserOnlyDto
                               {
                                   Id = c.User.Id,
                                   Name = c.User.Name,
                                   SurName = c.User.SurName,
                                   Age = c.User.Age
                               }).ToList()
                               : new List<UserOnlyDto>()
            };

            var successResult = ApiResponse<CourseGetDto>.SuccesResult(courseDto);
            return successResult;

        }

        public async Task<ApiResponse<string>> UpdateCourseAsync(CoursePutDto input)
        {
            var courseQueryable = await _repositories.CourseRepository.GetCoursesAsync();
            var course = await courseQueryable.AsQueryable()
                                      .Include(c => c.UsersCourses)
                                            .ThenInclude(uc => uc.User)
                                      .Include(c => c.CoursesLecturers)
                                            .ThenInclude(cl => cl.Lecturer)
                                      .Where(c => c.Id == input.Id)
                                      .FirstOrDefaultAsync();

            if (input.Id == null)
            {
                throw new CustomExceptions.BadRequestException("Course Id is null");
            }

            course.CourseName = input.CourseName;
            course.FacultyId = input.FacultyId.HasValue ? input.FacultyId : null;


            await _repositories.CourseRepository.UpdateCourseAsync(course);


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
            }

            if (course == null)
            {
                throw new CustomExceptions.NoContentException("Course not found");
            }

            await _repositories.CourseRepository.SaveChangesAsync();
            var successResponse = ApiResponse<string>.SuccesResult("Course changed successfully");
            return successResponse;

        }

        public async Task<ApiResponse<string>> DeleteCourse(int courseId)
        {
            var course = await _context.Courses
                                 .Include(uc => uc.UsersCourses)
                                        .ThenInclude(u => u.User)
                                 .Include(cl => cl.CoursesLecturers)
                                        .ThenInclude(l => l.Lecturer)
                                 .FirstOrDefaultAsync(c => c.Id == courseId);
            if(course == null)
            {
                throw new CustomExceptions.NotFoundException("Course not found on that Id");
            }

            if (await _repositories.CourseRepository.DeleteCourseAsync(courseId) &&
                       await _repositories.CourseRepository.DeleteUsersCoursesAsync(courseId) &&
                       await _repositories.CourseRepository.DeleteCourseLecturersAsync(courseId))
            {
                await _repositories.CourseRepository.SaveChangesAsync();
                var succesResult = ApiResponse<string>.SuccesResult("Course deleted successfully");
                return succesResult;
            }
            else
            {
                throw new Exception();
            }
            
            
        }
    }
}
