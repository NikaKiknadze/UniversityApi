using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.Services.Lecturers;
using University.Application.Services.Users;
using University.Domain.Models;
using University.Domain.Models.LecturerModels;
using University.Domain.Models.UserModels;

namespace University.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ManagementController(
    IUserServices userServices,
    ILecturerServices lecturerServices) : ControllerBase
{
    #region Users

    [HttpGet("Users")]
    public async Task<ActionResult<GetDtoWithCount<UserGetDto>>> GetUsersAsync([FromQuery] UserGetFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await userServices.Get(filter, cancellationToken));
    }

    [HttpPost("Users")]
    public async Task<ActionResult<UserGetDto>> PostUsersAsync(UserPostDto input, CancellationToken cancellationToken)
    {
        return Ok(await userServices.Create(input, cancellationToken));
    }

    [HttpPut("Users")]
    public async Task<ActionResult<bool>> PutUserAsync(UserPutDto input, CancellationToken cancellationToken)
    {
        return Ok(await userServices.Update(input, cancellationToken));
    }

    [HttpDelete("Users")]
    public async Task<ActionResult<bool>> DeleteUserAsync(int userId, CancellationToken cancellationToken)
    {
        return Ok(await userServices.Delete(userId, cancellationToken));
    }

    #endregion

    #region Lecturers

    [HttpGet("Lecturers")]
    public async Task<ActionResult> GetLecturersAsync([FromQuery]LecturerGetFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await lecturerServices.Get(filter, cancellationToken));
    }

    [HttpPost("Lecturers")]
    public async Task<ActionResult<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input, CancellationToken cancellationToken)
    {
        return Ok(await lecturerServices.Create(input, cancellationToken));
    }

    [HttpPut("Lecturers")]
    public async Task<ActionResult<bool>> PutLecturerAsync(LecturerPutDto input, CancellationToken cancellationToken)
    {
        return Ok(await lecturerServices.Update(input, cancellationToken));
    }

    [HttpDelete("Lecturers")]
    public async Task<ActionResult<bool>> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken)
    {
        return Ok(await lecturerServices.Delete(lecturerId, cancellationToken));
    }

    #endregion
}