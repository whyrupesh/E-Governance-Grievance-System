using Xunit;
using Microsoft.EntityFrameworkCore;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Backend.DTOs.Admin;
using Backend.Enums;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Tests.Services
{
    public class AdminServiceTests
    {
        private readonly AppDbContext _context;
        private readonly AdminService _service;

        public AdminServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new AdminService(_context);
        }

        [Fact]
        public async Task CreateDepartment_ShouldAddDepartment()
        {
            // Arrange
            var dto = new CreateDepartmentDto { Name = "IT", Description = "Tech Support" };

            // Act
            await _service.CreateDepartmentAsync(dto);

            // Assert
            var dept = await _context.Departments.FirstOrDefaultAsync();
            Assert.NotNull(dept);
            Assert.Equal("IT", dept.Name);
            Assert.Equal("Tech Support", dept.Description);
        }

        [Fact]
        public async Task CreateCategory_ShouldAddCategory_WhenDepartmentExists()
        {
            // Arrange
            var dept = new Department { Name = "HR", Description = "Human Resources" };
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();

            var dto = new CreateCategoryDto { Name = "Leave", DepartmentId = dept.Id };

            // Act
            await _service.CreateCategoryAsync(dto);

            // Assert
            var category = await _context.Categories.FirstOrDefaultAsync();
            Assert.NotNull(category);
            Assert.Equal("Leave", category.Name);
            Assert.Equal(dept.Id, category.DepartmentId);
        }

        [Fact]
        public async Task CreateCategory_ShouldThrowException_WhenDepartmentDoesNotExist()
        {
            // Arrange
            var dto = new CreateCategoryDto { Name = "Payroll", DepartmentId = 999 };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CreateCategoryAsync(dto));
        }

        [Fact]
        public async Task CreateOfficer_ShouldAddOfficer()
        {
             // Arrange
            var dept = new Department { Name = "Security", Description = "Sec" };
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();

            var dto = new CreateOfficerDto 
            { 
                FullName = "Officer John", 
                Email = "john@officer.com", 
                Password = "password", 
                DepartmentId = dept.Id 
            };

            // Act
            await _service.CreateOfficerAsync(dto);

            // Assert
            var officer = await _context.Users.FirstOrDefaultAsync(u => u.Email == "john@officer.com");
            Assert.NotNull(officer);
            Assert.Equal(UserRole.Officer, officer.Role);
            Assert.Equal(dept.Id, officer.DepartmentId);
        }

        [Fact]
        public async Task CreateSupervisor_ShouldAddSupervisor()
        {
            // Arrange
            var dto = new CreateSupervisorDto 
            { 
                FullName = "Sup Jane", 
                Email = "jane@sup.com", 
                Password = "password" 
            };

            // Act
            await _service.CreateSupervisorAsync(dto);

            // Assert
            var supervisor = await _context.Users.FirstOrDefaultAsync(u => u.Email == "jane@sup.com");
            Assert.NotNull(supervisor);
            Assert.Equal(UserRole.Supervisor, supervisor.Role);
        }
    }
}
