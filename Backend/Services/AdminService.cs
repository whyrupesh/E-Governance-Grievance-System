using Backend.Data;
using Backend.DTOs.Admin;
using Backend.Enums;
using Backend.Helpers;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        return await _context.Departments.Select(g => new DepartmentResponseDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        }).
        ToListAsync();

    }

    public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesAsync()
    {
        return await _context.Categories
            .Include(c => c.Department)
            .Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                DepartmentId = c.DepartmentId,
                DepartmentName = c.Department.Name
            })
            .ToListAsync();
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

    public async Task<IEnumerable<OfficerResponseDto>> GetOfficersAsync()
    {
        return await _context.Users
            .Where(u => u.Role == UserRole.Officer)
            .Select(u => new OfficerResponseDto
            {
                Id = u.Id,
                Name = u.FullName,
                Email = u.Email,
                Department = u.DepartmentId.ToString(),
                Role = u.Role.ToString()
            })
            .ToListAsync();
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

    public async Task DeleteOfficerAsync(int id)
    {
        var officer = await _context.Users.FindAsync(id);
        if (officer == null)
            throw new Exception("Officer not found");

        _context.Users.Remove(officer);
        await _context.SaveChangesAsync();
    }
}
