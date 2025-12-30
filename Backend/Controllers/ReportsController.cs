using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Admin,Supervisor")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("status-count")]
    public async Task<IActionResult> StatusWiseCount()
    {
        var result = await _reportService.GetGrievanceCountByStatusAsync();
        return Ok(result);
    }

    [HttpGet("department-performance")]
    public async Task<IActionResult> DepartmentPerformance()
    {
        var result = await _reportService.GetDepartmentPerformanceAsync();
        return Ok(result);
    }
}
