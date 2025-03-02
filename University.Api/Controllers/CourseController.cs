using Microsoft.AspNetCore.Mvc;
using University.Application.Services.Courses;
using University.Domain.Models;

namespace University.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController(ICourseServices courseServices) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCoursesAsync([FromQuery]CourseGetFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await courseServices.Get(filter, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourseAsync(CoursePostDto input, CancellationToken cancellationToken)
    {
        return Ok(await courseServices.Create(input, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> PutCourseAsync(CoursePutDto input, CancellationToken cancellationToken)
    {
        return Ok(await courseServices.Update(input, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCourse(int courseId, CancellationToken cancellationToken)
    {
        return Ok(await courseServices.Delete(courseId, cancellationToken));
    }
}