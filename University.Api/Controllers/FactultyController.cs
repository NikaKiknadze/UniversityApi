using Microsoft.AspNetCore.Mvc;
using University.Application.AllServices.ServiceAbstracts;
using University.Domain.Models;

namespace University.Api.Controllers
{
    [Route("api/Faculties")]
    [ApiController]
    public class FactultyController(IFacultyServices faultyServices)
        : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetDtoWithCount<FacultyGetDto>>> GetFacultiesAsync([FromQuery]FacultyGetFilter filter, CancellationToken cancellationToken)
        {
            return Ok(await faultyServices.GetFacultiesAsync(filter, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<FacultyGetDto>> PostFacultyAsync(FacultyPostDto input, CancellationToken cancellationToken)
        {
            return Ok(await faultyServices.CreateFacultyAsync(input, cancellationToken));
        }

        [HttpPut]
        public async Task<ActionResult<bool>> PutFacultyAsync(FacultyPutDto input, CancellationToken cancellationToken)
        {
            return Ok(await faultyServices.UpdateFacultyAsync(input, cancellationToken));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken)
        {
            return Ok(await faultyServices.DeleteFacultyAsync(facultyId, cancellationToken));
        }
    }
}
