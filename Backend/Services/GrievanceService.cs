using Backend.Data;
using Backend.DTOs.Grievance;
using Backend.Enums;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class GrievanceService : IGrievanceService
{
    private readonly AppDbContext _context;

    public GrievanceService(AppDbContext context)
    {
        _context = context;
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
            Status = GrievanceStatus.Submitted
        };

        _context.Grievances.Add(grievance);
        await _context.SaveChangesAsync();

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
                CreatedAt = g.CreatedAt
            })
            .ToListAsync();
    }
}
