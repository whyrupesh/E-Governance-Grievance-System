using Backend.DTOs.Admin;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Backend.Services;

public interface IAdminService
{
    Task CreateDepartmentAsync(CreateDepartmentDto dto);
    Task<IEnumerable<DepartmentResponseDto>> GetDepartmentAsync();
    Task CreateCategoryAsync(CreateCategoryDto dto);
    Task<IEnumerable<CategoryResponseDto>> GetCategoriesAsync();

    Task<IEnumerable<OfficerResponseDto>> GetOfficersAsync();
    Task CreateOfficerAsync(CreateOfficerDto dto);
    Task DeleteOfficerAsync(int id);
}
