using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using UniversityApi.Dtos;
using UniversityApi.Services;

namespace UniversityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly UserServices _userServices;
        private readonly CourseServices _courseServices;
        private readonly LecturerServices _lecturerServices;
        private readonly FacultyServices _faultyServices;

        public UniversityController(UserServices userServices, CourseServices courseServices, LecturerServices lecturerServices, FacultyServices faultyServices)
        {
            _userServices = userServices;
            _courseServices = courseServices;
            _lecturerServices = lecturerServices;
            _faultyServices = faultyServices;
        }

        #region UserServices
        [HttpGet("Users/{userId}", Name = "GetUsersById")]
        public ActionResult<UserGetDto> GetUsersById(int userId)
        {
            var result = _userServices.GetUserById(userId);

            if (result == null)
            {
                NotFound();
            }

            return Ok(result);

        }

        [HttpGet("Users", Name = "GetUsers")]
        public ActionResult<UserGetDto> GetUsers()
        {
            var result = _userServices.GetUsers();

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Users", Name = "PostUser" )]
        public ActionResult<UserGetDto> PostUsesr(UserPostDto input)
        {
            var result = _userServices.CreateUser(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPut("Users", Name = "PutUser")]
        public ActionResult<bool> PutUser(UserPutDto input)
        {
            var result = _userServices.UpdateUser(input);

            if (result == null)
            {
                NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("Users/{userId}", Name = "DeleteUser")]
        public ActionResult<bool> DeleteUser(int userId)
        {
            var result = _userServices.DeleteUser(userId);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }
        #endregion

        #region CourseServices
        [HttpGet("Courses", Name = "GetCourses")]
        public ActionResult<CourseGetDto>  GetCourses()
        {
            var result = _courseServices.GetCourses();

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Courses", Name = "CreateCourse")]
        public ActionResult<CourseGetDto> CreateCourse(CoursePostDto input)
        {
            var result = _courseServices.CreateCourse(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPut("Courses", Name = "PutCourse")]
        public ActionResult<bool> PutCourse(CoursePutDto input)
        {
            var result = _courseServices.UpdateCourse(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("Courses/{courseId}", Name = "DeleteCourse")]
        public ActionResult<bool> DeleteCourse(int courseId)
        {
            var result = _courseServices.DeleteCourse(courseId);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        #endregion

        #region LecturerServices
        [HttpGet("Lecturers", Name = "GetLecturers")]
        public ActionResult  GetLecturers()
        {
            var result = _lecturerServices.GetLecturers();

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Lecturers", Name = "CreateLecturer")]
        public ActionResult<LecturerGetDto> CreateLecturer(LecturerPostDto input)
        {
            var result = _lecturerServices.CreateLecturer(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPut("Lecturers", Name = "PutLecturer")]
        public ActionResult<bool> PutLecturer(LecturerPutDto input)
        {
            var result = _lecturerServices.UpdateLecturer(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("Lecturers/{lecturerId}", Name = "DeleteLecturer")]
        public ActionResult<bool> DeleteLecturer(int lecturerId)
        {
            var result = _lecturerServices.DeleteLecturer(lecturerId);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }
        #endregion

        #region FacultyServices
        [HttpGet("Faculties/{facultyId}", Name = "GetFacultiesById")]
        public ActionResult<FacultyGetDto> GetFacultyesById(int facultyId)
        {
            var result = _faultyServices.GetFacultyById(facultyId);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpGet("Faculties", Name = "GetFaculties")]
        public ActionResult<FacultyGetDto> GetFaculties()
        {
            var result = _faultyServices.GetFaculties();

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Faculties", Name = "PostFaculties")]
        public ActionResult<FacultyGetDto> PostFaculty(FacultyPostDto input)
        {
            var result = _faultyServices.CreateFaculty(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPut("Faculties", Name = "PutFaculty")]
        public ActionResult<bool> PutFaculty(FacultyPutDto input)
        {
            var result = _faultyServices.UpdateFaculty(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("Faculties/{facultyId}", Name = "DeleteFaculty")]
        public ActionResult<bool> DeleteFaculty(int facultyId)
        {
            var result = _faultyServices.DeleteFaculty(facultyId);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }
        #endregion

    }
}
