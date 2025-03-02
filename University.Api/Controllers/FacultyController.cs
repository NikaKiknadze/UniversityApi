using Microsoft.AspNetCore.Mvc;
using University.Application.Services.Faculties;
using University.Domain.Models;
using University.Domain.Models.FacultyModels;

namespace University.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController(IFacultyServices faultyServices)
        : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetDtoWithCount<FacultyGetDto>>> GetFacultiesAsync([FromQuery]FacultyGetFilter filter, CancellationToken cancellationToken)
        {
            return Ok(await faultyServices.Get(filter, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<FacultyGetDto>> PostFacultyAsync(FacultyPostDto input, CancellationToken cancellationToken)
        {
            return Ok(await faultyServices.Create(input, cancellationToken));
        }

        [HttpPut]
        public async Task<ActionResult<bool>> PutFacultyAsync(FacultyPutDto input, CancellationToken cancellationToken)
        {
            return Ok(await faultyServices.Update(input, cancellationToken));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteFacultyAsync(int facultyId, CancellationToken cancellationToken)
        {
            return Ok(await faultyServices.Delete(facultyId, cancellationToken));
        }
    }
}
