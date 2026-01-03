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

    [HttpGet("departments")]
    public async Task<IActionResult> GetDepartment()
    {
        var result = await _adminService.GetDepartmentAsync();
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _adminService.GetCategoriesAsync();
        return Ok(result);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
    {
        await _adminService.CreateCategoryAsync(dto);
        return Ok();
    }

    [HttpGet("officers")]
    public async Task<IActionResult> GetOfficers()
    {
        var result = await _adminService.GetOfficersAsync();
        return Ok(result);
    }

    [HttpPost("officers")]
    public async Task<IActionResult> CreateOfficer(CreateOfficerDto dto)
    {
        await _adminService.CreateOfficerAsync(dto);
        return Ok();
    }
}
