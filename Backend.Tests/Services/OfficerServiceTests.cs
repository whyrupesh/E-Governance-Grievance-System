using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Backend.DTOs.Officer;
using Backend.Enums;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Tests.Services
{
    public class OfficerServiceTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly OfficerService _service;

        public OfficerServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _mockNotificationService = new Mock<INotificationService>();
            _service = new OfficerService(_context, _mockNotificationService.Object);
        }

        [Fact]
        public async Task GetAssignedGrievances_ShouldReturnGrievancesForDepartment()
        {
            // Arrange
            var dept = new Department { Id = 1, Name = "IT", Description = "IT Dept" };
            _context.Departments.Add(dept);

            var officer = new User { Id = 10, FullName = "Officer One", Email = "off@test.com", PasswordHash = "p", Role = UserRole.Officer, DepartmentId = dept.Id };
            _context.Users.Add(officer);

            var category = new Category { Id = 1, Name = "Hardware", DepartmentId = dept.Id };
            _context.Categories.Add(category);

            var citizen = new User { Id = 1, FullName = "Cit", Email = "c@t.com", PasswordHash = "h", Role = UserRole.Citizen };
            _context.Users.Add(citizen);

            await _context.SaveChangesAsync(); // Save dependencies first

            _context.Grievances.Add(new Grievance
            {
                Id = 1,
                DepartmentId = dept.Id,
                CategoryId = category.Id,
                Status = GrievanceStatus.InReview,
                GrievanceNumber = "G001",
                Description = "Grievance 1",
                CitizenId = citizen.Id,
                AssignedTo = "Unassigned",
                Feedback = ""
            });

            // Grievance in different department
            var hrDept = new Department { Id = 2, Name = "HR", Description = "HR" };
            _context.Departments.Add(hrDept);
            await _context.SaveChangesAsync();

            _context.Grievances.Add(new Grievance
            {
                Id = 2,
                DepartmentId = hrDept.Id,
                CategoryId = category.Id, // Same category
                Status = GrievanceStatus.InReview,
                GrievanceNumber = "G002",
                Description = "Grievance 2",
                CitizenId = citizen.Id,
                AssignedTo = "Unassigned",
                Feedback = ""
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAssignedGrievancesAsync(officer.Id);

            // Assert
            Assert.Single(result);
            Assert.Equal("G001", result.First().GrievanceNumber);
        }

        [Fact]
        public async Task UpdateStatus_ShouldUpdateAndNotify_WhenAuthorized()
        {
            // Arrange
            var dept = new Department { Id = 3, Name = "Admin", Description = "Admin" };
            _context.Departments.Add(dept);

            var officer = new User { Id = 20, FullName = "Officer Two", Email = "off2@test.com", PasswordHash = "p", Role = UserRole.Officer, DepartmentId = dept.Id };
            _context.Users.Add(officer);

            var citizen = new User { Id = 21, FullName = "Citizen Two", Email = "cit2@test.com", PasswordHash = "p", Role = UserRole.Citizen };
            _context.Users.Add(citizen);

            var supervisor = new User { Id = 22, FullName = "Supervisor Two", Email = "sup2@test.com", PasswordHash = "p", Role = UserRole.Supervisor };
            _context.Users.Add(supervisor);

            var category = new Category { Id = 2, Name = "General", DepartmentId = dept.Id };
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            var grievance = new Grievance
            {
                Id = 10,
                DepartmentId = dept.Id,
                CategoryId = category.Id,
                Status = GrievanceStatus.InReview,
                GrievanceNumber = "G010",
                CitizenId = citizen.Id,
                Description = "Desc",
                AssignedTo = "Unassigned",
                Feedback = ""
            };
            _context.Grievances.Add(grievance);
            await _context.SaveChangesAsync();

            var dto = new UpdateGrievanceStatusDto { Status = GrievanceStatus.Resolved, ResolutionRemarks = "Done" };

            // Act
            await _service.UpdateStatusAsync(grievance.Id, officer.Id, dto);

            // Assert
            var updated = await _context.Grievances.FindAsync(grievance.Id);
            Assert.Equal(GrievanceStatus.Resolved, updated.Status);
            Assert.Equal("Done", updated.ResolutionRemarks);
            Assert.NotNull(updated.ResolvedAt);

            // Verify notifications (Citizen + Supervisor)
            _mockNotificationService.Verify(x => x.CreateNotificationAsync(citizen.Id, It.IsAny<string>(), grievance.Id), Times.Once);
            _mockNotificationService.Verify(x => x.CreateNotificationAsync(supervisor.Id, It.IsAny<string>(), grievance.Id), Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_ShouldThrow_WhenOfficerUnauthorized()
        {
            // Arrange
            var dept = new Department { Id = 4, Name = "Logistics", Description = "Log" };
            _context.Departments.Add(dept);

            var officer = new User { Id = 30, FullName = "Officer Three", Email = "off3@test.com", PasswordHash = "p", Role = UserRole.Officer, DepartmentId = dept.Id };
            _context.Users.Add(officer);

            var otherDept = new Department { Id = 5, Name = "HR", Description = "HR" };
            _context.Departments.Add(otherDept);

            var citizen = new User { Id = 31, FullName = "Cit 3", Email = "c3@c.com", Role = UserRole.Citizen, PasswordHash = "p" };
            _context.Users.Add(citizen);

            var category = new Category { Id = 3, Name = "Deliv", DepartmentId = otherDept.Id };
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            var grievance = new Grievance
            {
                Id = 11,
                DepartmentId = otherDept.Id, // Different Dept
                CategoryId = category.Id,
                Status = GrievanceStatus.InReview,
                GrievanceNumber = "G011",
                CitizenId = citizen.Id,
                Description = "Desc",
                AssignedTo = "Unassigned",
                Feedback = ""
            };
            _context.Grievances.Add(grievance);
            await _context.SaveChangesAsync();

            var dto = new UpdateGrievanceStatusDto { Status = GrievanceStatus.Resolved };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateStatusAsync(grievance.Id, officer.Id, dto));
        }
    }
}
