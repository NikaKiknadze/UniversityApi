using Microsoft.AspNetCore.Mvc;
using University.Application.AllServices.ServiceAbstracts;
using University.Domain.Models;

namespace University.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly ICourseServices _courseServices;
        private readonly IFacultyServices _faultyServices;
        private readonly ILecturerServices _lecturerServices;
        private readonly IUserServices _userServices;
        private readonly IHierarchyService _hierarchyService;

        public UniversityController(IUserServices userServices, 
                                    ICourseServices courseServices, 
                                    ILecturerServices lecturerServices, 
                                    IFacultyServices faultyServices,
                                    IHierarchyService hierarchyService)
        {
            _userServices = userServices;
            _courseServices = courseServices;
            _lecturerServices = lecturerServices;
            _faultyServices = faultyServices;
            _hierarchyService = hierarchyService;
        }

        #region UserServices
        [HttpGet("Users", Name = "GetUsers")]
        public async Task<ActionResult<GetDtoWithCount<UserGetDto>>> GetUsersAsync([FromQuery] UserGetFilter filter, CancellationToken cancellationToken)
        {
            return Ok(await _userServices.GetUsersAsync(filter, cancellationToken));

        }

        [HttpPost("Users", Name = "PostUser")]
        public async Task<ActionResult<UserGetDto>> PostUsesrAsync(UserPostDto input, CancellationToken cancellationToken)
        {
            return Ok(await _userServices.CreateUserAsync(input, cancellationToken));
        }

        [HttpPut("Users", Name = "PutUser")]
        public async Task<ActionResult<bool>> PutUserAsync(UserPutDto input, CancellationToken cancellationToken)
        {
            return Ok(await _userServices.UpdateUserAsync(input, cancellationToken));
        }

        [HttpDelete("Users/{userId}", Name = "DeleteUser")]
        public async Task<ActionResult<bool>> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            return Ok(await _userServices.DeleteUserAsync(userId, cancellationToken));
        }

        [HttpGet("Todos", Name = "GetTodosInfo")]
        public async Task<ActionResult<GetDtoWithCount<IEnumerable<TodosDto>>>> GetTodosInfo([FromQuery]TodosDto filter, CancellationToken cancellationToken)
        {
            return Ok(await _userServices.GetTodosInfo(filter, cancellationToken));
        }
        #endregion

        #region CourseServices
        [HttpGet("Courses", Name = "GetCourses")]
        public async Task<ActionResult<GetDtoWithCount<CourseGetDto>>> GetCoursesAsync([FromQuery]CourseGetFilter filter, CancellationToken cancellationToken)
        {
            return Ok(await _courseServices.GetCoursesAsync(filter, cancellationToken));
        }

        [HttpPost("Courses", Name = "CreateCourse")]
        public async Task<ActionResult<CourseGetDto>> CreateCourseAsync(CoursePostDto input, CancellationToken cancellationToken)
        {
            return Ok(await _courseServices.CreateCourseAsync(input, cancellationToken));
        }

        [HttpPut("Courses", Name = "PutCourse")]
        public async Task<ActionResult<bool>> PutCourseAsync(CoursePutDto input, CancellationToken cancellationToken)
        {
            return Ok(await _courseServices.UpdateCourseAsync(input, cancellationToken));
        }

        [HttpDelete("Courses/{courseId}", Name = "DeleteCourse")]
        public async Task<ActionResult<bool>> DeleteCourse(int courseId, CancellationToken cancellationToken)
        {
            return Ok(await _courseServices.DeleteCourse(courseId, cancellationToken));
        }

        #endregion

        #region LecturerServices
        [HttpGet("Lecturers", Name = "GetLecturers")]
        public async Task<ActionResult> GetLecturersAsync([FromQuery]LecturerGetFilter filter, CancellationToken cancellationToken)
        {
            return Ok(await _lecturerServices.GetLecturersAsync(filter, cancellationToken));
        }

        [HttpPost("Lecturers", Name = "CreateLecturer")]
        public async Task<ActionResult<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input, CancellationToken cancellationToken)
        {
            return Ok(await _lecturerServices.CreateLecturerAsync(input, cancellationToken));
        }

        [HttpPut("Lecturers", Name = "PutLecturer")]
        public async Task<ActionResult<bool>> PutLecturerAsync(LecturerPutDto input, CancellationToken cancellationToken)
        {
            return Ok(await _lecturerServices.UpdateLecturerAsync(input, cancellationToken));
        }

        [HttpDelete("Lecturers/{lecturerId}", Name = "DeleteLecturer")]
        public async Task<ActionResult<bool>> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken)
        {
            return Ok(await _lecturerServices.DeleteLecturerAsync(lecturerId, cancellationToken));
        }
        #endregion

        #region FacultyServices

        [HttpGet("Faculties", Name = "GetFaculties")]
        public async Task<ActionResult<GetDtoWithCount<FacultyGetDto>>> GetFacultiesAsync([FromQuery]FacultyGetFilter filter, CancellationToken cancellationToken)
        {
            return Ok(await _faultyServices.GetFacultiesAsync(filter, cancellationToken));
        }

        [HttpPost("Faculties", Name = "PostFaculties")]
        public async Task<ActionResult<FacultyGetDto>> PostFacultyAsync(FacultyPostDto input, CancellationToken cancellationToken)
        {
            return Ok(await _faultyServices.CreateFacultyAsync(input, cancellationToken));
        }

        [HttpPut("Faculties", Name = "PutFaculty")]
        public async Task<ActionResult<bool>> PutFacultyAsync(FacultyPutDto input, CancellationToken cancellationToken)
        {
            return Ok(await _faultyServices.UpdateFacultyAsync(input, cancellationToken));
        }

        [HttpDelete("Faculties/{facultyId}", Name = "DeleteFaculty")]
        public async Task<ActionResult<bool>> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken)
        {
            return Ok(await _faultyServices.DeleteFacultyAsync(facultyId, cancellationToken));
        }
        #endregion

        #region HierarchyServices

        [HttpGet("Hierarchy", Name = "GetHyerarchyObjects")]
        public async Task<ActionResult<GetDtoWithCount<HierarchyDto>>> GetHierarchyObjectsAsync([FromQuery] HierarchyDto filter, CancellationToken cancellationToken)
        {
            return Ok(await _hierarchyService.GetHierarchyObjectsAsync(filter, cancellationToken));
        }

        [HttpPost("Hierarchy", Name = "PostHierarchyObject")]
        public async Task<ActionResult<HierarchyDto>> PostHierarchyObjectAsync(HierarchyDto input, CancellationToken cancellationToken)
        {
            return Ok(await _hierarchyService.CreateHierarchyObjectAsync(input, cancellationToken));
        }

        [HttpPut("Hierarchy", Name = "PutHierarchyObject")]
        public async Task<ActionResult<bool>> PutHierarchyAsync(HierarchyDto input, CancellationToken cancellationToken)
        {
            return Ok(await _hierarchyService.UpdateHierarchyObjectAsync(input, cancellationToken));
        }

        [HttpDelete("Hierarchy/{hierarchyId}", Name = "DeleteHierarchyObject")]
        public async Task<ActionResult<bool>> DeleteHierarchyAsync(int hierarchyId, CancellationToken cancellationToken)
        {
            return Ok(await _hierarchyService.DeleteHierarchyObjectAsync(hierarchyId, cancellationToken));
        }
        #endregion

    }
}
