using Backend.DTOs.Reports;

namespace Backend.Services;

public interface IReportService
{
    Task<IEnumerable<StatusCountDto>> GetGrievanceCountByStatusAsync(int? departmentId = null);
    Task<IEnumerable<DepartmentPerformanceDto>> GetDepartmentPerformanceAsync(int? departmentId = null);
    Task<IEnumerable<CategoryCountDto>> GetGrievanceCountByCategoryAsync(int? departmentId = null);
}
