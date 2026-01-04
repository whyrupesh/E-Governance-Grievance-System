using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/supervisor")]
[Authorize(Roles = "Supervisor")]
public class SupervisorController : ControllerBase
{
    private readonly ISupervisorService _supervisorService;

    public SupervisorController(ISupervisorService supervisorService)
    {
        _supervisorService = supervisorService;
    }

    [HttpGet("grievances")]
    public async Task<IActionResult> AllGrievances()
    {
        var result = await _supervisorService.GetAllGrievances();

        return Ok(result);
    }

    [HttpGet("grievances/{id}")]
    public async Task<IActionResult> GetGrievanceById(int id)
    {
        var result = await _supervisorService.GetGrievanceByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("overdue")]
    public async Task<IActionResult> OverdueGrievances([FromQuery] int days = 7)
    {
        var result = await _supervisorService.GetOverdueGrievancesAsync(days);
        return Ok(result);
    }

    [HttpPost("grievances/{id}/status")]
    public async Task<IActionResult> ChangeGrievanceStatus(int id, [FromBody] string status)
    {
        var result = await _supervisorService.ChangeGrievanceStatusAsync(id, status);
        return Ok(result);
    }

    [HttpPost("grievances/{id}/remarks")]
    public async Task<IActionResult> GiveResolutionRemarks(int id, [FromBody] string remarks)
    {
        var result = await _supervisorService.GiveResolutionRemarksAsync(id, remarks);
        return Ok(result);
    }


    [HttpPost("grievances/{id}/assign")]
    public async Task<IActionResult> AssignGrievance(int id, [FromBody] int staffId)
    {
        var result = await _supervisorService.AssignGrievanceAsync(id, staffId);
        return Ok(result);
    }

    [HttpGet("departments/{id}/officers")]
    public async Task<IActionResult> GetOfficersByDepartment(int id)
    {
        var result = await _supervisorService.GetOfficersByDepartmentAsync(id);
        return Ok(result);
    }

    [HttpPatch("escalate/{id}")]
    public async Task<IActionResult> Escalate(int id)
    {
        await _supervisorService.EscalateAsync(id);
        return NoContent();
    }
}
