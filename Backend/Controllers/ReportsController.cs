using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Admin,Supervisor,Officer")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    private int? GetDepartmentId()
    {
        if (User.IsInRole("Admin")) return null;

        var deptClaim = User.FindFirst("DepartmentId");
        if (deptClaim != null && int.TryParse(deptClaim.Value, out int deptId))
        {
            return deptId;
        }
        return null; // Should ideally not happen for Officer/Supervisor if logic is correct
    }

    [HttpGet("status-count")]
    public async Task<IActionResult> StatusWiseCount()
    {
        var result = await _reportService.GetGrievanceCountByStatusAsync(GetDepartmentId());
        return Ok(result);
    }

    [HttpGet("department-performance")]
    public async Task<IActionResult> DepartmentPerformance()
    {
        var result = await _reportService.GetDepartmentPerformanceAsync(GetDepartmentId());
        return Ok(result);
    }

    [HttpGet("category-count")]
    public async Task<IActionResult> CategoryWiseCount()
    {
        var result = await _reportService.GetGrievanceCountByCategoryAsync(GetDepartmentId());
        return Ok(result);
    }
}
