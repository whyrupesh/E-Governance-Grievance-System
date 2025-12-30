using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/secure")]
public class SecureController : ControllerBase
{
    [Authorize]
    [HttpGet("user")]
    public IActionResult UserAccess()
    {
        return Ok("Authenticated user");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok("Admin access");
    }
}
