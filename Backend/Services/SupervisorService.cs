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
            EscalatedAt = grievance.EscalatedAt,
            AssignedTo = grievance.AssignedTo,
            Feedback = grievance.Feedback
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

    public async Task<GrievanceResponseDto> ChangeGrievanceStatusAsync(int grievanceId, string status)
    {
        var grievance = await _context.Grievances.FindAsync(grievanceId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        if (!Enum.TryParse<GrievanceStatus>(status, out var newStatus))
            throw new Exception("Invalid status");

        grievance.Status = newStatus;

        if (newStatus == GrievanceStatus.Resolved)
            grievance.ResolvedAt = DateTime.UtcNow;

        _context.Grievances.Update(grievance);
        await _context.SaveChangesAsync();

        return await GetGrievanceByIdAsync(grievanceId);    
    }

    public async Task<GrievanceResponseDto> GiveResolutionRemarksAsync(int grievanceId, string remarks)
    {
        var grievance = await _context.Grievances.FindAsync(grievanceId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        grievance.ResolutionRemarks = remarks;
        grievance.Status = GrievanceStatus.Closed;
        grievance.ResolvedAt = DateTime.UtcNow;

        _context.Grievances.Update(grievance);
        await _context.SaveChangesAsync();

        return await GetGrievanceByIdAsync(grievanceId);    
    }

    public async Task<GrievanceResponseDto> AssignGrievanceAsync(int grievanceId, int staffId)
    {
        var grievance = await _context.Grievances.FindAsync(grievanceId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        var staff = await _context.Users.FindAsync(staffId);
        if (staff == null || staff.Role != UserRole.Officer)
            throw new Exception("Invalid staff member");

        grievance.AssignedTo = staff.FullName;
        grievance.Status = GrievanceStatus.Assigned;

        _context.Grievances.Update(grievance);
        await _context.SaveChangesAsync();

        return await GetGrievanceByIdAsync(grievanceId);    
    }

    public async Task<IEnumerable<object>> GetOfficersByDepartmentAsync(int DepartmentId)
    {
        var officers = await _context.Users
            .Where(u => u.DepartmentId == DepartmentId && u.Role == UserRole.Officer)
            .Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email
            })
            .ToListAsync();

        return officers;
    }


}
