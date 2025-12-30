using Backend.Data;
using Backend.DTOs.Officer;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class OfficerService : IOfficerService
{
    private readonly AppDbContext _context;

    public OfficerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetAssignedGrievancesAsync(int officerId)
    {
        var officer = await _context.Users.FindAsync(officerId);

        if (officer == null || officer.DepartmentId == null)
            throw new Exception("Officer not assigned to any department");

        return await _context.Grievances
            .Where(g => g.DepartmentId == officer.DepartmentId)
            .Include(g => g.Category)
            .Select(g => new
            {
                g.Id,
                g.GrievanceNumber,
                Category = g.Category.Name,
                g.Status,
                g.CreatedAt
            })
            .ToListAsync();
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

        grievance.Status = dto.Status;

        if (dto.Status == GrievanceStatus.Resolved)
        {
            grievance.ResolvedAt = DateTime.UtcNow;
            grievance.ResolutionRemarks = dto.ResolutionRemarks;
        }

        await _context.SaveChangesAsync();
    }
}
