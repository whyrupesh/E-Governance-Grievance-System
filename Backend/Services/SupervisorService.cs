using Backend.Data;
using Backend.DTOs.Grievance;
using Backend.Enums;
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
            Department = g.Department.Name,
            Category = g.Category.Name,
            Description = g.Description,
            Status = g.Status.ToString(),
            CreatedAt = g.CreatedAt,
            IsEscalated = g.IsEscalated
        }).ToListAsync();
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
            return;

        grievance.IsEscalated = true;
        grievance.EscalatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}
