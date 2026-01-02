using Backend.Data;
using Backend.DTOs.Reports;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    // 1️⃣ Grievance count by status
    public async Task<IEnumerable<StatusCountDto>> GetGrievanceCountByStatusAsync()
    {
        return await _context.Grievances
            .GroupBy(g => g.Status)
            .Select(g => new StatusCountDto
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
    }

    // 2️⃣ Department performance
    public async Task<IEnumerable<DepartmentPerformanceDto>> GetDepartmentPerformanceAsync()
    {
        var grievances = await _context.Grievances
            .Include(g => g.Department)
            .Where(g => g.ResolvedAt != null)
            .ToListAsync();

        return grievances
            .GroupBy(g => g.Department.Name)
            .Select(group => new DepartmentPerformanceDto
            {
                Department = group.Key,
                TotalGrievances = group.Count(),
                ResolvedGrievances = group.Count(),

                AverageResolutionDays = Math.Round(
                    group.Average(g =>
                        (g.ResolvedAt!.Value - g.CreatedAt).TotalDays
                    ),
                    2
                )
            })
            .ToList();
    }

}
