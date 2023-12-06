using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualBasic;
using System.Net;
using UniversityApi.CustomResponses;
using UniversityApi.Dtos;
using UniversityApi.Service.ServiceAbstracts;
using UniversityApi.Service.Services;

namespace UniversityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly ICourseServices _courseServices;
        private readonly IFacultyServices _faultyServices;
        private readonly ILecturerServices _lecturerServices;
        private readonly IUserServices _userServices;

        public UniversityController(IUserServices userServices, ICourseServices courseServices, ILecturerServices lecturerServices, IFacultyServices faultyServices)
        {
            _userServices = userServices;
            _courseServices = courseServices;
            _lecturerServices = lecturerServices;
            _faultyServices = faultyServices;
        }

        #region UserServices
        [HttpGet("Users/{userId}", Name = "GetUsersById")]
        public async Task<ActionResult<UserGetDto>> GetUsersByIdAsync(int userId)
        {
            var result = await _userServices.GetUserByIdAsync(userId);
            return Ok(result);

        }

        [HttpGet("Users", Name = "GetUsers")]
        public async Task<ActionResult<UserGetDto>> GetUsersAsync()
        {
            var result = await _userServices.GetUsersAsync();
            return Ok(result);
        }

        [HttpPost("Users", Name = "PostUser")]
        public async Task<ActionResult<UserGetDto>> PostUsesrAsync(UserPostDto input)
        {
            var result = await _userServices.CreateUserAsync(input);
            return Ok(result);
        }

        [HttpPut("Users", Name = "PutUser")]
        public async Task<ActionResult<bool>> PutUserAsync(UserPutDto input)
        {
            var result = await _userServices.UpdateUserAsync(input);
            return Ok(result);
        }

        [HttpDelete("Users/{userId}", Name = "DeleteUser")]
        public async Task<ActionResult<bool>> DeleteUserAsync(int userId)
        {
            var result = await _userServices.DeleteUserAsync(userId);
            return Ok(result);
        }
        #endregion

        #region CourseServices
        [HttpGet("Courses", Name = "GetCourses")]
        public async Task<ActionResult<CourseGetDto>> GetCoursesAsync()
        {
            var result = await _courseServices.GetCoursesAsync();
            return Ok(result);
        }

        [HttpPost("Courses", Name = "CreateCourse")]
        public async Task<ActionResult<CourseGetDto>> CreateCourseAsync(CoursePostDto input)
        {
            var result = await _courseServices.CreateCourseAsync(input);
            return Ok(result);
        }

        [HttpPut("Courses", Name = "PutCourse")]
        public async Task<ActionResult<bool>> PutCourseAsync(CoursePutDto input)
        {
            var result = await _courseServices.UpdateCourseAsync(input);
            return Ok(result);
        }

        [HttpDelete("Courses/{courseId}", Name = "DeleteCourse")]
        public async Task<ActionResult<bool>> DeleteCourse(int courseId)
        {
            var result = await _courseServices.DeleteCourse(courseId);
            return Ok(result);
        }

        #endregion

        #region LecturerServices
        [HttpGet("Lecturers", Name = "GetLecturers")]
        public async Task<ActionResult> GetLecturersAsync()
        {
            var result = await _lecturerServices.GetLecturersAsync();
            return Ok(result);
        }

        [HttpPost("Lecturers", Name = "CreateLecturer")]
        public async Task<ActionResult<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input)
        {
            var result = await _lecturerServices.CreateLecturerAsync(input);
            return Ok(result);
        }

        [HttpPut("Lecturers", Name = "PutLecturer")]
        public async Task<ActionResult<bool>> PutLecturerAsync(LecturerPutDto input)
        {
            var result = await _lecturerServices.UpdateLecturerAsync(input);
            return Ok(result);
        }

        [HttpDelete("Lecturers/{lecturerId}", Name = "DeleteLecturer")]
        public async Task<ActionResult<bool>> DeleteLecturerAsync(int lecturerId)
        {
            var result = await _lecturerServices.DeleteLecturerAsync(lecturerId);
            return Ok(result);
        }
        #endregion

        #region FacultyServices
        [HttpGet("Faculties/{facultyId}", Name = "GetFacultiesById")]
        public async Task<ActionResult<FacultyGetDto>> GetFacultyesByIdAsync(int facultyId)
        {
            var result = await _faultyServices.GetFacultyByIdAsync(facultyId);
            return Ok(result);
        }

        [HttpGet("Faculties", Name = "GetFaculties")]
        public async Task<ActionResult<FacultyGetDto>> GetFacultiesAsync()
        {
            var result = await _faultyServices.GetFacultiesAsync();
            return Ok(result);
        }

        [HttpPost("Faculties", Name = "PostFaculties")]
        public async Task<ActionResult<FacultyGetDto>> PostFacultyAsync(FacultyPostDto input)
        {
            var result = await _faultyServices.CreateFacultyAsync(input);
            return Ok(result);
        }

        [HttpPut("Faculties", Name = "PutFaculty")]
        public async Task<ActionResult<bool>> PutFacultyAsync(FacultyPutDto input)
        {
            var result = await _faultyServices.UpdateFacultyAsync(input);
            return Ok(result);
        }

        [HttpDelete("Faculties/{facultyId}", Name = "DeleteFaculty")]
        public async Task<ActionResult<bool>> DeleteFacultyAsync(int facultyId)
        {
            var result = await _faultyServices.DeleteFacultyAsync(facultyId);
            return Ok(result);
        }
        #endregion

    }
}
