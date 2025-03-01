using Microsoft.AspNetCore.Mvc;
using University.Application.AllServices.ServiceAbstracts;
using University.Domain.Models;

namespace University.Api.Controllers;

public class ManagementController(
    IUserServices userServices,
    IHierarchyService hierarchyService,
    ILecturerServices lecturerServices) : ControllerBase
{
    #region Users

    [HttpGet("Users", Name = "GetUsers")]
    public async Task<ActionResult<GetDtoWithCount<UserGetDto>>> GetUsersAsync([FromQuery] UserGetFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await userServices.GetUsersAsync(filter, cancellationToken));
    }

    [HttpPost("Users", Name = "PostUser")]
    public async Task<ActionResult<UserGetDto>> PostUsesrAsync(UserPostDto input, CancellationToken cancellationToken)
    {
        return Ok(await userServices.CreateUserAsync(input, cancellationToken));
    }

    [HttpPut("Users", Name = "PutUser")]
    public async Task<ActionResult<bool>> PutUserAsync(UserPutDto input, CancellationToken cancellationToken)
    {
        return Ok(await userServices.UpdateUserAsync(input, cancellationToken));
    }

    [HttpDelete("Users/{userId}", Name = "DeleteUser")]
    public async Task<ActionResult<bool>> DeleteUserAsync(int userId, CancellationToken cancellationToken)
    {
        return Ok(await userServices.DeleteUserAsync(userId, cancellationToken));
    }

    [HttpGet("Todos", Name = "GetTodosInfo")]
    public async Task<ActionResult<GetDtoWithCount<IEnumerable<TodosDto>>>> GetTodosInfo([FromQuery]TodosDto filter, CancellationToken cancellationToken)
    {
        return Ok(await userServices.GetTodosInfo(filter, cancellationToken));
    }

    #endregion
    
    #region HierarchyServices

    [HttpGet("Hierarchy", Name = "GetHyerarchyObjects")]
    public async Task<ActionResult<GetDtoWithCount<HierarchyDto>>> GetHierarchyObjectsAsync([FromQuery] HierarchyDto filter, CancellationToken cancellationToken)
    {
        return Ok(await hierarchyService.Get(filter, cancellationToken));
    }

    [HttpPost("Hierarchy", Name = "PostHierarchyObject")]
    public async Task<ActionResult<HierarchyDto>> PostHierarchyObjectAsync(HierarchyDto input, CancellationToken cancellationToken)
    {
        return Ok(await hierarchyService.Create(input, cancellationToken));
    }

    [HttpPut("Hierarchy", Name = "PutHierarchyObject")]
    public async Task<ActionResult<bool>> PutHierarchyAsync(HierarchyDto input, CancellationToken cancellationToken)
    {
        return Ok(await hierarchyService.Update(input, cancellationToken));
    }

    [HttpDelete("Hierarchy/{hierarchyId}", Name = "DeleteHierarchyObject")]
    public async Task<ActionResult<bool>> DeleteHierarchyAsync(int hierarchyId, CancellationToken cancellationToken)
    {
        return Ok(await hierarchyService.Delete(hierarchyId, cancellationToken));
    }
    #endregion

    #region Lecturers

    [HttpGet("Lecturers", Name = "GetLecturers")]
    public async Task<ActionResult> GetLecturersAsync([FromQuery]LecturerGetFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await lecturerServices.GetLecturersAsync(filter, cancellationToken));
    }

    [HttpPost("Lecturers", Name = "CreateLecturer")]
    public async Task<ActionResult<LecturerGetDto>> CreateLecturerAsync(LecturerPostDto input, CancellationToken cancellationToken)
    {
        return Ok(await lecturerServices.CreateLecturerAsync(input, cancellationToken));
    }

    [HttpPut("Lecturers", Name = "PutLecturer")]
    public async Task<ActionResult<bool>> PutLecturerAsync(LecturerPutDto input, CancellationToken cancellationToken)
    {
        return Ok(await lecturerServices.UpdateLecturerAsync(input, cancellationToken));
    }

    [HttpDelete("Lecturers/{lecturerId}", Name = "DeleteLecturer")]
    public async Task<ActionResult<bool>> DeleteLecturerAsync(int lecturerId, CancellationToken cancellationToken)
    {
        return Ok(await lecturerServices.DeleteLecturerAsync(lecturerId, cancellationToken));
    }

    #endregion
}