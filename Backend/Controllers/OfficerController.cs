using Backend.DTOs.Officer;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/officer")]
[Authorize(Roles = "Officer")]
public class OfficerController : ControllerBase
{
    private readonly IOfficerService _officerService;

    public OfficerController(IOfficerService officerService)
    {
        _officerService = officerService;
    }

    [HttpGet("grievances")]
    public async Task<IActionResult> AssignedGrievances()
    {
        var officerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _officerService.GetAssignedGrievancesAsync(officerId);
        return Ok(result);
    }

    [HttpGet("grievances/{id}")]
    public async Task<IActionResult> GetGrievanceById(int id)
    {
        var officerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _officerService.GetGrievanceByIdAsync(id, officerId);
        return Ok(result);
    }

    [HttpPut("grievances/{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateGrievanceStatusDto dto)
    {
        Console.WriteLine("UpdateStatus called");
        var officerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _officerService.UpdateStatusAsync(id, officerId, dto);
        return NoContent();
    }
}
