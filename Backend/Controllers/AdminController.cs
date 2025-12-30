using Backend.DTOs.Admin;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("departments")]
    public async Task<IActionResult> CreateDepartment(CreateDepartmentDto dto)
    {
        await _adminService.CreateDepartmentAsync(dto);
        return Ok();
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
    {
        await _adminService.CreateCategoryAsync(dto);
        return Ok();
    }

    [HttpPost("officers")]
    public async Task<IActionResult> CreateOfficer(CreateOfficerDto dto)
    {
        await _adminService.CreateOfficerAsync(dto);
        return Ok();
    }
}
