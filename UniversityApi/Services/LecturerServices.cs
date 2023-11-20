using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public List<LecturerGetDto> GetLecturers()
        {
            var lecturers = _lecturerRepository.GetLecturersWithRelatedData();

            var lecturerDtos = lecturers.Select(lecturer => new LecturerGetDto
            {
                Id = lecturer.Id,
                Name = lecturer.Name,
                SurName = lecturer.SurName,
                Age = lecturer.Age,
                UserIds = lecturer.UsersLecturers?.Select(l => l.UserId).ToList() ?? new List<int>(),
                CourseIds = lecturer.CoursesLecturers?.Select(l => l.CourseId).ToList() ?? new List<int>()
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
                lecturer.CoursesLecturers= new List<CoursesLecturersJoin>();

                foreach(var courseId in input.CourseIds)
                {
                    lecturer.CoursesLecturers.Add(new CoursesLecturersJoin()
                    {
                        CourseId = courseId,
                        LectureId = lecturer.Id
                    });
                }
            }

            if(!input.UserIds.IsNullOrEmpty())
            {
                lecturer.UsersLecturers = new List<UsersLecturersJoin>();

                foreach(var userId in input.UserIds)
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
                UserIds = lecturer.UsersLecturers.Select(l => l.UserId).ToList(),
                CourseIds = lecturer.CoursesLecturers.Select(l => l.CourseId).ToList()
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
                    foreach(var userId in input.UserIds)
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
                    foreach(var courseId in input.CourseIds)
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

        public bool DeleteLecturer(LecturerDeleteDto input)
        {
            if(_lecturerRepository.DeleteLecturer(input.Id) &&
               _lecturerRepository.DeleteUsersLecturers(input.Id) &&
               _lecturerRepository.DeleteCoursesLecturers(input.Id))
            {
                _lecturerRepository.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
