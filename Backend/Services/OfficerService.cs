using Backend.Data;
using Backend.DTOs.Grievance;
using Backend.DTOs.Officer;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class OfficerService : IOfficerService
{
    private readonly AppDbContext _context;
    private readonly INotificationService _notificationService;

    public OfficerService(AppDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<GrievanceResponseDto>> GetAssignedGrievancesAsync(int officerId)
    {
        var officer = await _context.Users.FindAsync(officerId);

        if (officer == null || officer.DepartmentId == null)
            throw new Exception("Officer not assigned to any department");

        return await _context.Grievances
            .Where(g => g.DepartmentId == officer.DepartmentId && g.Status != GrievanceStatus.Submitted)
            .Include(g => g.Category)
            .Select(g => new GrievanceResponseDto
            {
                Id = g.Id,
                GrievanceNumber = g.GrievanceNumber,
                Category = g.Category.Name,
                Department = g.Department.Name,
                CitizenId = g.CitizenId,
                DepartmentId = g.DepartmentId,
                CategoryId = g.CategoryId,
                Status = g.Status.ToString(),
                CreatedAt = g.CreatedAt,
                Description = g.Description,
                ResolvedAt = g.ResolvedAt,
                ResolutionRemarks = g.ResolutionRemarks ?? "",
                IsEscalated = g.IsEscalated,
                EscalatedAt = g.EscalatedAt
            })
            .ToListAsync();
    }

    public async Task<GrievanceResponseDto> GetGrievanceByIdAsync(int grievanceId, int officerId)
    {
        var officer = await _context.Users.FindAsync(officerId);
        if (officer?.DepartmentId == null)
            throw new Exception("Unauthorized officer");

        var grievance = await _context.Grievances
            .Include(g => g.Category)
            .Include(g => g.Department)
            .FirstOrDefaultAsync(g => g.Id == grievanceId);

        if (grievance == null)
            throw new Exception("Grievance not found");

        if (grievance.DepartmentId != officer.DepartmentId)
            throw new Exception("Access denied");

        return new GrievanceResponseDto
        {
            Id = grievance.Id,
            GrievanceNumber = grievance.GrievanceNumber,
            Category = grievance.Category.Name,
            Department = grievance.Department.Name,
            CitizenId = grievance.CitizenId,
            DepartmentId = grievance.DepartmentId,
            CategoryId = grievance.CategoryId,
            Status = grievance.Status.ToString(),
            CreatedAt = grievance.CreatedAt,
            Description = grievance.Description,
            ResolvedAt = grievance.ResolvedAt,
            ResolutionRemarks = grievance.ResolutionRemarks ?? "",
            IsEscalated = grievance.IsEscalated,
            EscalatedAt = grievance.EscalatedAt
        };
    }

    public async Task UpdateStatusAsync(int grievanceId, int officerId, UpdateGrievanceStatusDto dto)
    {
        var officer = await _context.Users.FindAsync(officerId);
        if (officer?.DepartmentId == null)
            throw new Exception("Unauthorized officer");

        var grievance = await _context.Grievances.FindAsync(grievanceId);
        if (grievance == null)
            throw new Exception("Grievance not found");

        if (grievance.DepartmentId != officer.DepartmentId)
            throw new Exception("Access denied");

        Console.WriteLine(dto.ResolutionRemarks);
        Console.WriteLine(dto.Status);

        var oldStatus = grievance.Status;
        grievance.Status = dto.Status;

        if (dto.Status == GrievanceStatus.Resolved)
        {
            grievance.ResolvedAt = DateTime.UtcNow;
            grievance.ResolutionRemarks = dto.ResolutionRemarks;
        }

        await _context.SaveChangesAsync();

        if (oldStatus != dto.Status)
        {
            // Notify Citizen
            await _notificationService.CreateNotificationAsync(
                grievance.CitizenId,
                $"Your grievance #{grievance.GrievanceNumber} status has been updated to {dto.Status}.",
                grievance.Id
            );

            // Notify Supervisors
            var supervisors = await _context.Users
                .Where(u => u.Role == UserRole.Supervisor)
                .ToListAsync();

            foreach (var supervisor in supervisors)
            {
                await _notificationService.CreateNotificationAsync(
                   supervisor.Id,
                   $"Grievance #{grievance.GrievanceNumber} status updated to {dto.Status} by {officer.FullName}.",
                   grievance.Id
               );
            }
        }
    }
}
