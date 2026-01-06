using Xunit;
using Microsoft.EntityFrameworkCore;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Backend.Enums;
using Backend.DTOs.Grievance;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Backend.Tests.Services
{
    public class SupervisorServiceTests
    {
        private readonly AppDbContext _context;
        private readonly SupervisorService _service;

        public SupervisorServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new SupervisorService(_context);
        }

        [Fact]
        public async Task GetAllGrievances_ShouldReturnAll()
        {
            var dept = new Department { Id = 1, Name = "D", Description = "D" };
            var cat = new Category { Id = 1, Name = "C", DepartmentId = 1 };

            _context.Departments.Add(dept);
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            _context.Grievances.AddRange(
                new Grievance { Id = 1, Status = GrievanceStatus.Submitted, CategoryId = 1, DepartmentId = 1, Description = "D", CitizenId = 1, AssignedTo = "A", Feedback = "", GrievanceNumber = "G1" }
            );

            await _context.SaveChangesAsync();

            var result = await _service.GetAllGrievances();
            Assert.Single(result);
        }

        [Fact]
        public async Task AssignGrievance_ShouldUpdateAssignmentAndStatus()
        {
            // Arrange
            var dept = new Department { Id = 1, Name = "D", Description = "D" };
            var cat = new Category { Id = 1, Name = "C", DepartmentId = 1 };

            var officer = new User { Id = 10, FullName = "Off", Role = UserRole.Officer, Email = "o", PasswordHash = "p" };

            _context.Departments.Add(dept);
            _context.Categories.Add(cat);
            _context.Users.Add(officer);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var grievance = new Grievance
            {
                Id = 1,
                Status = GrievanceStatus.Submitted,
                CategoryId = 1,
                DepartmentId = 1,
                Description = "D",
                CitizenId = 1,
                AssignedTo = "Unassigned",
                Feedback = "",
                GrievanceNumber = "G1"
            };
            _context.Grievances.Add(grievance);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.AssignGrievanceAsync(grievance.Id, officer.Id);

            // Assert
            Assert.Equal("Off", result.AssignedTo);
            Assert.Equal("Assigned", result.Status);

            var dbGrievance = await _context.Grievances.FindAsync(grievance.Id);
            Assert.Equal(GrievanceStatus.Assigned, dbGrievance.Status);
        }

        [Fact]
        public async Task Escalate_ShouldToggleEscalation()
        {
            var dept = new Department { Id = 1, Name = "D", Description = "D" };
            var cat = new Category { Id = 1, Name = "C", DepartmentId = 1 };

            _context.Departments.Add(dept);
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var grievance = new Grievance
            {
                Id = 1,
                Status = GrievanceStatus.Submitted,
                CategoryId = 1,
                DepartmentId = 1,
                Description = "D",
                CitizenId = 1,
                AssignedTo = "U",
                Feedback = "",
                IsEscalated = false,
                GrievanceNumber = "G1"
            };
            _context.Grievances.Add(grievance);
            await _context.SaveChangesAsync();

            // Act
            await _service.EscalateAsync(grievance.Id);

            // Assert
            var updated = await _context.Grievances.FindAsync(grievance.Id);
            Assert.True(updated.IsEscalated);

            // Toggle back
            await _service.EscalateAsync(grievance.Id);
            Assert.False(updated.IsEscalated);
        }

        [Fact]
        public async Task GetOverdueGrievances_ShouldReturnOldGrievances()
        {
            var dept = new Department { Id = 1, Name = "D", Description = "D" };
            var cat = new Category { Id = 1, Name = "C", DepartmentId = 1 };

            _context.Departments.Add(dept);
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            _context.Grievances.Add(new Grievance
            {
                Id = 1,
                Status = GrievanceStatus.Submitted,
                CategoryId = 1,
                DepartmentId = 1,
                Description = "Old",
                CitizenId = 1,
                AssignedTo = "U",
                Feedback = "",
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                GrievanceNumber = "G1"
            });
            _context.Grievances.Add(new Grievance
            {
                Id = 2,
                Status = GrievanceStatus.Submitted,
                CategoryId = 1,
                DepartmentId = 1,
                Description = "New",
                CitizenId = 1,
                AssignedTo = "U",
                Feedback = "",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                GrievanceNumber = "G2"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetOverdueGrievancesAsync(5); // Older than 5 days

            // Assert
            Assert.Single(result);
        }
    }
}
