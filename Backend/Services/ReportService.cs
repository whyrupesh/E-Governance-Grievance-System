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
    public async Task<IEnumerable<StatusCountDto>> GetGrievanceCountByStatusAsync(int? departmentId = null)
    {
        var query = _context.Grievances.AsQueryable();

        if (departmentId.HasValue)
        {
            query = query.Where(g => g.DepartmentId == departmentId.Value);
        }

        return await query
            .GroupBy(g => g.Status)
            .Select(g => new StatusCountDto
            {
                Status = g.Key.ToString(),
                Count = g.Count()
            })
            .ToListAsync();
    }

    // 2️⃣ Department performance
    public async Task<IEnumerable<DepartmentPerformanceDto>> GetDepartmentPerformanceAsync(int? departmentId = null)
    {
        var query = _context.Grievances
            .Include(g => g.Department)
            .Where(g => g.ResolvedAt != null)
            .AsQueryable();

        if (departmentId.HasValue)
        {
            query = query.Where(g => g.DepartmentId == departmentId.Value);
        }

        var grievances = await query.ToListAsync();

        return grievances
            .GroupBy(g => g.Department.Name)
            .Select(group => new DepartmentPerformanceDto
            {
                Department = group.Key,
                TotalGrievances = group.Count(),
                ResolvedGrievances = group.Count(g => g.Status == GrievanceStatus.Resolved || g.Status == GrievanceStatus.Closed),
                PendingGrievances = group.Count(g => g.Status == GrievanceStatus.Submitted || g.Status == GrievanceStatus.Assigned || g.Status == GrievanceStatus.InReview),
                AverageResolutionDays = Math.Round(
                    group.Where(g => g.ResolvedAt != null)
                         .Select(g => (g.ResolvedAt!.Value - g.CreatedAt).TotalDays)
                         .DefaultIfEmpty(0)
                         .Average(),
                    2
                )
            })
            .ToList();
    }

    // 3️⃣ Grievance count by category
    public async Task<IEnumerable<CategoryCountDto>> GetGrievanceCountByCategoryAsync(int? departmentId = null)
    {
        var query = _context.Grievances
            .Include(g => g.Category)
            .AsQueryable();

        if (departmentId.HasValue)
        {
            query = query.Where(g => g.DepartmentId == departmentId.Value);
        }

        return await query
            .GroupBy(g => g.Category.Name)
            .Select(g => new CategoryCountDto
            {
                Category = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
    }
}
