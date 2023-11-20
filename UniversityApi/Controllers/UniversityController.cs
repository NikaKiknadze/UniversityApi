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

        [HttpGet("Users")]
        public ActionResult<UserGetDto> GetUsers()
        {
            var result = _userServices.GetUsers();

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Users")]
        public ActionResult<UserGetDto> PostUsesr(UserPostDto input)
        {
            var result = _userServices.CreateUser(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPut("Users")]
        public ActionResult<bool> PutUser(UserPutDto input)
        {
            var result = _userServices.UpdateUser(input);

            if (result == null)
            {
                NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("Users")]
        public ActionResult<bool> DeleteUser(UserDeleteDto input)
        {
            var result = _userServices.DeleteUser(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }
        #endregion

        #region CourseServices
        [HttpGet("Courses")]
        public ActionResult<CourseGetDto>  GetCourses()
        {
            var result = _courseServices.GetCourses();

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Courses")]
        public ActionResult<CourseGetDto> CreateCourse(CoursePostDto input)
        {
            var result = _courseServices.CreateCourse(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPut("Courses")]
        public ActionResult<bool> PutCourse(CoursePutDto input)
        {
            var result = _courseServices.UpdateCourse(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("Courses")]
        public ActionResult<bool> DeleteCourse(CourseDeleteDto input)
        {
            var result = _courseServices.DeleteCourse(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        #endregion

        #region LecturerServices
        [HttpGet("Lecturers")]
        public ActionResult  GetLecturers()
        {
            var result = _lecturerServices.GetLecturers();

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Lecturers")]
        public ActionResult<LecturerGetDto> CreateLecturer(LecturerPostDto input)
        {
            var result = _lecturerServices.CreateLecturer(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPut("Lecturers")]
        public ActionResult<bool> PutLecturer(LecturerPutDto input)
        {
            var result = _lecturerServices.UpdateLecturer(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("Lecturers")]
        public ActionResult<bool> DeleteLecturer(LecturerDeleteDto input)
        {
            var result = _lecturerServices.DeleteLecturer(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }
        #endregion

        #region FacultyServices
        [HttpGet("Faculties/{facultyId}")]
        public ActionResult<FacultyGetDto> GetFacultyesById(int facultyId)
        {
            var result = _faultyServices.GetFacultyById(facultyId);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpGet("Faculties")]
        public ActionResult<FacultyGetDto> GetFaculties()
        {
            var result = _faultyServices.GetFaculties();

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPost("Faculties")]
        public ActionResult<FacultyGetDto> PostFaculty(FacultyPostDto input)
        {
            var result = _faultyServices.CreateFaculty(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpPut("Faculties")]
        public ActionResult<bool> PutFaculty(FacultyPutDto input)
        {
            var result = _faultyServices.UpdateFaculty(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("Faculties")]
        public ActionResult<bool> DeleteFaculty(FacultyDeleteDto input)
        {
            var result = _faultyServices.DeleteFaculty(input);

            if (result == null)
            {
                NotFound();
            }
            return Ok(result);
        }
        #endregion


    }
}
