using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualBasic;
using System.Net;
using System.Threading;
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
        [HttpGet("Users", Name = "GetUsers")]
        public async Task<ActionResult<GetDtosWithCount<UserGetDto>>> GetUsersAsync([FromQuery] UserGetFilter filter, CancellationToken cancellationToken)
        {
            var result = await _userServices.GetUsersAsync(filter, cancellationToken);
            return Ok(result);

        }

        [HttpPost("Users", Name = "PostUser")]
        public async Task<ActionResult<UserGetDto>> PostUsesrAsync(UserPostDto input, CancellationToken cancellationToken)
        {
            var result = await _userServices.CreateUserAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpPut("Users", Name = "PutUser")]
        public async Task<ActionResult<bool>> PutUserAsync(UserPutDto input, CancellationToken cancellationToken)
        {
            var result = await _userServices.UpdateUserAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("Users/{userId}", Name = "DeleteUser")]
        public async Task<ActionResult<bool>> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var result = await _userServices.DeleteUserAsync(userId, cancellationToken);
            return Ok(result);
        }
        #endregion

        #region CourseServices
        [HttpGet("Courses", Name = "GetCourses")]
        public async Task<ActionResult<GetDtosWithCount<CourseGetDto>>> GetCoursesAsync([FromQuery]CourseGetFilter filter, CancellationToken cancellationToken)
        {
            var result = await _courseServices.GetCoursesAsync(filter, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Courses", Name = "CreateCourse")]
        public async Task<ActionResult<CourseGetDto>> CreateCourseAsync(CoursePostDto input, CancellationToken cancellationToken)
        {
            var result = await _courseServices.CreateCourseAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpPut("Courses", Name = "PutCourse")]
        public async Task<ActionResult<bool>> PutCourseAsync(CoursePutDto input, CancellationToken cancellationToken)
        {
            var result = await _courseServices.UpdateCourseAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("Courses/{courseId}", Name = "DeleteCourse")]
        public async Task<ActionResult<bool>> DeleteCourse(int courseId, CancellationToken cancellationToken)
        {
            var result = await _courseServices.DeleteCourse(courseId, cancellationToken);
            return Ok(result);
        }

        #endregion

        #region LecturerServices
        [HttpGet("Lecturers", Name = "GetLecturers")]
        public async Task<ActionResult> GetLecturersAsync([FromQuery]LecturerGetFilter filter, CancellationToken cancellationToken)
        {
            var result = await _lecturerServices.GetLecturersAsync(filter, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Lecturers", Name = "CreateLecturer")]
        public async Task<ActionResult<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input, CancellationToken cancellationToken)
        {
            var result = await _lecturerServices.CreateLecturerAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpPut("Lecturers", Name = "PutLecturer")]
        public async Task<ActionResult<bool>> PutLecturerAsync(LecturerPutDto input, CancellationToken cancellationToken)
        {
            var result = await _lecturerServices.UpdateLecturerAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("Lecturers/{lecturerId}", Name = "DeleteLecturer")]
        public async Task<ActionResult<bool>> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken)
        {
            var result = await _lecturerServices.DeleteLecturerAsync(lecturerId, cancellationToken);
            return Ok(result);
        }
        #endregion

        #region FacultyServices

        [HttpGet("Faculties", Name = "GetFaculties")]
        public async Task<ActionResult<GetDtosWithCount<FacultyGetDto>>> GetFacultiesAsync([FromQuery]FacultyGetFilter filter, CancellationToken cancellationToken)
        {
            var result = await _faultyServices.GetFacultiesAsync(filter, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Faculties", Name = "PostFaculties")]
        public async Task<ActionResult<FacultyGetDto>> PostFacultyAsync(FacultyPostDto input, CancellationToken cancellationToken)
        {
            var result = await _faultyServices.CreateFacultyAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpPut("Faculties", Name = "PutFaculty")]
        public async Task<ActionResult<bool>> PutFacultyAsync(FacultyPutDto input, CancellationToken cancellationToken)
        {
            var result = await _faultyServices.UpdateFacultyAsync(input, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("Faculties/{facultyId}", Name = "DeleteFaculty")]
        public async Task<ActionResult<bool>> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken)
        {
            var result = await _faultyServices.DeleteFacultyAsync(facultyId, cancellationToken);
            return Ok(result);
        }
        #endregion

    }
}
