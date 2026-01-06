using Backend.Data;
using Backend.DTOs.Admin;
using Backend.DTOs.Grievance;
using Backend.Enums;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class GrievanceService : IGrievanceService
{
    private readonly AppDbContext _context;
    private readonly INotificationService _notificationService;

    public GrievanceService(AppDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<GrievanceResponseDto> CreateAsync(int citizenId, CreateGrievanceDto dto)
    {
        var category = await _context.Categories
            .Include(c => c.Department)
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);

        if (category == null)
            throw new Exception("Invalid category");

        var grievance = new Grievance
        {
            CitizenId = citizenId,
            CategoryId = category.Id,
            DepartmentId = category.DepartmentId,
            Description = dto.Description,
            GrievanceNumber = $"GRV-{DateTime.UtcNow.Ticks}",
            Status = GrievanceStatus.Submitted,
            AssignedTo = "",
            Feedback = ""

        };

        _context.Grievances.Add(grievance);
        await _context.SaveChangesAsync();

        // Notify Citizen
        await _notificationService.CreateNotificationAsync(
            citizenId,
            $"Your grievance #{grievance.GrievanceNumber} has been successfully submitted.",
            grievance.Id
        );

        // Notify Supervisors (All Supervisors)
        var supervisors = await _context.Users
            .Where(u => u.Role == UserRole.Supervisor)
            .ToListAsync();

        foreach (var supervisor in supervisors)
        {
            await _notificationService.CreateNotificationAsync(
               supervisor.Id,
               $"New Grievance #{grievance.GrievanceNumber} submitted in category {category.Name}.",
               grievance.Id
           );
        }

        // Notify Officers (In the same Department)
        var officers = await _context.Users
            .Where(u => u.Role == UserRole.Officer && u.DepartmentId == category.DepartmentId)
            .ToListAsync();

        foreach (var officer in officers)
        {
            await _notificationService.CreateNotificationAsync(
               officer.Id,
               $"New Grievance #{grievance.GrievanceNumber} assigned to your department.",
               grievance.Id
           );
        }

        return new GrievanceResponseDto
        {
            Id = grievance.Id,
            GrievanceNumber = grievance.GrievanceNumber,
            Category = category.Name,
            Department = category.Department.Name,
            Status = grievance.Status.ToString(),
            CreatedAt = grievance.CreatedAt
        };
    }

    public async Task<IEnumerable<GrievanceResponseDto>> GetMyGrievancesAsync(int citizenId)
    {
        return await _context.Grievances
            .Where(g => g.CitizenId == citizenId)
            .Include(g => g.Category)
            .Include(g => g.Department)
            .OrderByDescending(g => g.CreatedAt)
            .Select(g => new GrievanceResponseDto
            {
                Id = g.Id,
                GrievanceNumber = g.GrievanceNumber,
                Category = g.Category.Name,
                Department = g.Department.Name,
                Status = g.Status.ToString(),
                CreatedAt = g.CreatedAt,
                Description = g.Description,
                ResolvedAt = g.ResolvedAt,
                ResolutionRemarks = g.ResolutionRemarks,
                IsEscalated = g.IsEscalated,
                EscalatedAt = g.EscalatedAt
            })
            .ToListAsync();
    }

    public async Task<GrievanceResponseDto?> GetMyGrievanceByIdAsync(int grievanceId, int citizenId)
    {
        return await _context.Grievances
            .Where(g => g.Id == grievanceId && g.CitizenId == citizenId)
            .Include(g => g.Category)
            .Include(g => g.Department)
            .Select(g => new GrievanceResponseDto
            {
                Id = g.Id,
                GrievanceNumber = g.GrievanceNumber,
                Category = g.Category.Name,
                Department = g.Department.Name,
                Status = g.Status.ToString(),
                CreatedAt = g.CreatedAt,
                Description = g.Description,
                ResolvedAt = g.ResolvedAt,
                ResolutionRemarks = g.ResolutionRemarks,
                IsEscalated = g.IsEscalated,
                EscalatedAt = g.EscalatedAt
            })
            .FirstOrDefaultAsync();
    }


    public async Task<object> DeleteMyGrievanceAsync(int grievanceId, int citizenId)
    {
        var grievance = await _context.Grievances
            .FirstOrDefaultAsync(g => g.Id == grievanceId && g.CitizenId == citizenId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        _context.Grievances.Remove(grievance);
        await _context.SaveChangesAsync();

        return new { message = "Grievance deleted successfully" };
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllCategories()
    {
        return await _context.Categories.Select(g => new CategoryResponseDto
        {
            Id = g.Id,
            Name = g.Name,
            // DepartmentId = g.DepartmentId
        }).ToListAsync();
    }

    public async Task AddFeedbackAsync(int grievanceId, int citizenId, AddFeedbackDto dto)
    {
        var grievance = await _context.Grievances
            .FirstOrDefaultAsync(g => g.Id == grievanceId && g.CitizenId == citizenId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        if (grievance.Status != GrievanceStatus.Closed)
            throw new Exception("Feedback can only be provided for Closed grievances");

        grievance.Feedback = dto.Feedback ?? "";
        grievance.Rating = dto.Rating;

        await _context.SaveChangesAsync();
    }

    public async Task ReopenGrievanceAsync(int grievanceId, int citizenId)
    {
        var grievance = await _context.Grievances
             .Include(g => g.Category)
            .FirstOrDefaultAsync(g => g.Id == grievanceId && g.CitizenId == citizenId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        if (grievance.Status != GrievanceStatus.Resolved)
            throw new Exception("Only resolved grievances can be reopened");

        if (grievance.ResolvedAt == null || (DateTime.UtcNow - grievance.ResolvedAt.Value).TotalDays > 7)
            throw new Exception("Grievance cannot be reopened after 7 days of resolution");

        grievance.Status = GrievanceStatus.Reopened;
        grievance.ReopenedAt = DateTime.UtcNow;
        // grievance.ResolutionRemarks += "\n[Reopened]: User is not satisfied."; 

        await _context.SaveChangesAsync();

        // Notify Supervisors
        var supervisors = await _context.Users
           .Where(u => u.Role == UserRole.Supervisor)
           .ToListAsync();

        foreach (var supervisor in supervisors)
        {
            await _notificationService.CreateNotificationAsync(
               supervisor.Id,
               $"Grievance #{grievance.GrievanceNumber} has been reopened by citizen.",
               grievance.Id
           );
        }

        // Notify Officers
        var officers = await _context.Users
            .Where(u => u.Role == UserRole.Officer && u.DepartmentId == grievance.DepartmentId)
            .ToListAsync();

        foreach (var officer in officers)
        {
            await _notificationService.CreateNotificationAsync(
               officer.Id,
               $"Grievance #{grievance.GrievanceNumber} has been reopened.",
               grievance.Id
           );
        }
    }
    public async Task EscalateGrievanceAsync(int grievanceId, int citizenId)
    {
        var grievance = await _context.Grievances
            .FirstOrDefaultAsync(g => g.Id == grievanceId && g.CitizenId == citizenId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        if (grievance.Status == GrievanceStatus.Resolved || grievance.Status == GrievanceStatus.Closed)
            throw new Exception("Cannot change escalation status of a resolved or closed grievance.");

        if (grievance.IsEscalated)
        {
            // De-escalate
            grievance.IsEscalated = false;
            // Optionally clear EscalatedAt or keep the last escalation time? Keeping it simple for now and just toggling flag.
            // Or maybe update it? Let's leave EscalatedAt as the time it was LAST escalated.
        }
        else
        {
            // Escalate
            grievance.IsEscalated = true;
            grievance.EscalatedAt = DateTime.UtcNow;

            // Notify Supervisors only on Escalation
            var supervisors = await _context.Users
               .Where(u => u.Role == UserRole.Supervisor)
               .ToListAsync();

            foreach (var supervisor in supervisors)
            {
                await _notificationService.CreateNotificationAsync(
                   supervisor.Id,
                   $"Grievance #{grievance.GrievanceNumber} has been ESCALATED by the citizen.",
                   grievance.Id
               );
            }
        }

        await _context.SaveChangesAsync();
    }
}
