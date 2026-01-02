using Backend.Data;
using Backend.DTOs.Admin;
using Backend.Enums;
using Backend.Helpers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;

    public AdminService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        _context.Departments.Add(new Department
        {
            Name = dto.Name,
            Description = dto.Description
        });

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DepartmentResponseDto>> GetDepartmentAsync()
    {
        return await _context.Departments.Select( g => new DepartmentResponseDto
        {
            Name = g.Name,
            Description = g.Description
        }).
        ToListAsync();

    }

    public async Task CreateCategoryAsync(CreateCategoryDto dto)
    {
        var departmentExists = await _context.Departments
            .AnyAsync(d => d.Id == dto.DepartmentId);

        if (!departmentExists)
            throw new Exception("Department not found");

        _context.Categories.Add(new Category
        {
            Name = dto.Name,
            DepartmentId = dto.DepartmentId
        });

        await _context.SaveChangesAsync();
    }

    public async Task CreateOfficerAsync(CreateOfficerDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Officer already exists");

        _context.Users.Add(new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Role = UserRole.Officer,
            DepartmentId = dto.DepartmentId
        });

        await _context.SaveChangesAsync();
    }
}
