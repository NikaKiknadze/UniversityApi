using Microsoft.AspNetCore.Mvc;
using University.Application.Services.Auth;
using University.Domain.Models.AuthModels;

namespace University.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthServices authServices) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthModel request, CancellationToken cancellationToken)
    {
        return Ok(await authServices.Login(request, cancellationToken));
    }
}