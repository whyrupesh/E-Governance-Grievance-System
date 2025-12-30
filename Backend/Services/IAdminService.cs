using Backend.DTOs.Admin;

namespace Backend.Services;

public interface IAdminService
{
    Task CreateDepartmentAsync(CreateDepartmentDto dto);
    Task CreateCategoryAsync(CreateCategoryDto dto);
    Task CreateOfficerAsync(CreateOfficerDto dto);
}
