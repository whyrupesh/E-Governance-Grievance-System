using Backend.DTOs.Reports;

namespace Backend.Services;

public interface IReportService
{
    Task<IEnumerable<StatusCountDto>> GetGrievanceCountByStatusAsync();
    Task<IEnumerable<DepartmentPerformanceDto>> GetDepartmentPerformanceAsync();
}
