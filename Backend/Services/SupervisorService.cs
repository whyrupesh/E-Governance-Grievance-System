using Backend.Data;
using Backend.DTOs.Grievance;
using Backend.Enums;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class SupervisorService : ISupervisorService
{
    private readonly AppDbContext _context;

    public SupervisorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GrievanceResponseDto>> GetAllGrievances()
    {
        return await _context.Grievances.Select(g => new GrievanceResponseDto
        {
            Id = g.Id,
            GrievanceNumber = g.GrievanceNumber,
            CitizenId = g.CitizenId,
            DepartmentId = g.DepartmentId,
            CategoryId = g.CategoryId,
            Department = g.Department.Name,
            Category = g.Category.Name,
            Description = g.Description,
            Status = g.Status.ToString(),
            CreatedAt = g.CreatedAt,
            IsEscalated = g.IsEscalated,
            ResolvedAt = g.ResolvedAt,
            ResolutionRemarks = g.ResolutionRemarks,
            EscalatedAt = g.EscalatedAt
        }).ToListAsync();
    }

    public async Task<GrievanceResponseDto> GetGrievanceByIdAsync(int grievanceId)
    {
        var grievance = await _context.Grievances
            .Include(g => g.Department)
            .Include(g => g.Category)
            .FirstOrDefaultAsync(g => g.Id == grievanceId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        return new GrievanceResponseDto
        {
            Id = grievance.Id,
            GrievanceNumber = grievance.GrievanceNumber,
            CitizenId = grievance.CitizenId,
            DepartmentId = grievance.DepartmentId,
            CategoryId = grievance.CategoryId,
            Department = grievance.Department.Name,
            Category = grievance.Category.Name,
            Description = grievance.Description,
            Status = grievance.Status.ToString(),
            CreatedAt = grievance.CreatedAt,
            IsEscalated = grievance.IsEscalated,
            ResolvedAt = grievance.ResolvedAt,
            ResolutionRemarks = grievance.ResolutionRemarks,
            EscalatedAt = grievance.EscalatedAt
        };
    }

    public async Task<IEnumerable<object>> GetOverdueGrievancesAsync(int days)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);

        var grievances = await _context.Grievances
            .Where(g =>
                g.Status != GrievanceStatus.Resolved &&
                g.Status != GrievanceStatus.Closed &&
                g.CreatedAt <= cutoffDate
            )
            .Include(g => g.Department)
            .Include(g => g.Category)
            .ToListAsync();

        return grievances
            .Select(g => new
            {
                g.Id,
                g.GrievanceNumber,
                Department = g.Department.Name,
                Category = g.Category.Name,
                g.Status,
                g.CreatedAt,
                DaysPending = Math.Floor(
                    (DateTime.UtcNow - g.CreatedAt).TotalDays
                ),
                g.IsEscalated
            })
            .OrderByDescending(g => g.DaysPending)
            .ToList();
    }


    public async Task EscalateAsync(int grievanceId)
    {
        var grievance = await _context.Grievances.FindAsync(grievanceId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        if (grievance.IsEscalated)
        {
            grievance.IsEscalated = false;
            grievance.EscalatedAt = DateTime.UtcNow;
        }
        else
            grievance.IsEscalated = true;

        _context.Grievances.Update(grievance);
        await _context.SaveChangesAsync();
    }




}
