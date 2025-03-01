using Microsoft.AspNetCore.Mvc;
using University.Application.AllServices.ServiceAbstracts;
using University.Domain.Models;

namespace University.Api.Controllers;

public class CourseController(ICourseServices courseServices) : ControllerBase
{
    [HttpGet("Courses", Name = "GetCourses")]
    public async Task<ActionResult<GetDtoWithCount<CourseGetDto>>> GetCoursesAsync([FromQuery]CourseGetFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await courseServices.GetCoursesAsync(filter, cancellationToken));
    }

    [HttpPost("Courses", Name = "CreateCourse")]
    public async Task<ActionResult<CourseGetDto>> CreateCourseAsync(CoursePostDto input, CancellationToken cancellationToken)
    {
        return Ok(await courseServices.CreateCourseAsync(input, cancellationToken));
    }

    [HttpPut("Courses", Name = "PutCourse")]
    public async Task<ActionResult<bool>> PutCourseAsync(CoursePutDto input, CancellationToken cancellationToken)
    {
        return Ok(await courseServices.UpdateCourseAsync(input, cancellationToken));
    }

    [HttpDelete("Courses/{courseId}", Name = "DeleteCourse")]
    public async Task<ActionResult<bool>> DeleteCourse(int courseId, CancellationToken cancellationToken)
    {
        return Ok(await courseServices.DeleteCourse(courseId, cancellationToken));
    }
}