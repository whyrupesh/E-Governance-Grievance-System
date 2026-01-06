using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Backend.Enums;
using Backend.DTOs.Grievance;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Backend.Tests.Services
{
    public class GrievanceServiceTests
    {
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly AppDbContext _context;
        private readonly GrievanceService _service;

        public GrievanceServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _context = new AppDbContext(options);
            _mockNotificationService = new Mock<INotificationService>();

            // Seed Category
            _context.Categories.Add(new Category { Id = 1, Name = "Test Category", DepartmentId = 1, Department = new Department { Id = 1, Name = "Test Dept", Description = "Test Dept Description" } });

            // Seed Supervisor
            _context.Users.Add(new User { Id = 201, FullName = "Supervisor One", Email = "sup@test.com", PasswordHash = "hash", Role = UserRole.Supervisor });

            _context.SaveChanges();

            _service = new GrievanceService(_context, _mockNotificationService.Object);
        }

        [Fact]
        public async Task LodgeGrievance_ShouldAddGrievance_WhenValid()
        {
            // Arrange
            var dto = new CreateGrievanceDto
            {
                Description = "Test Description",
                CategoryId = 1
            };
            int citizenId = 101;

            // Act
            var result = await _service.CreateAsync(citizenId, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(citizenId, await _context.Grievances.Where(g => g.Id == result.Id).Select(g => g.CitizenId).FirstOrDefaultAsync());
            Assert.Equal(GrievanceStatus.Submitted.ToString(), result.Status);

            var savedGrievance = await _context.Grievances.FindAsync(result.Id);
            Assert.NotNull(savedGrievance);
        }

        [Fact]
        public async Task EscalateGrievance_ShouldToggleEscalation_WhenAuthorized()
        {
            // Arrange
            var grievance = new Grievance
            {
                Description = "Desc",
                CitizenId = 101,
                Status = GrievanceStatus.Submitted,
                CreatedAt = DateTime.UtcNow,
                IsEscalated = false,
                CategoryId = 1,
                DepartmentId = 1,
                GrievanceNumber = "TEST-123",
                AssignedTo = "Unassigned",
                Feedback = ""
            };
            _context.Grievances.Add(grievance);
            await _context.SaveChangesAsync();

            // Act: Escalate
            await _service.EscalateGrievanceAsync(grievance.Id, 101);

            // Assert
            var escalatedGrievance = await _context.Grievances.FindAsync(grievance.Id);
            Assert.True(escalatedGrievance.IsEscalated);
            Assert.NotNull(escalatedGrievance.EscalatedAt);

            // Verify notification was sent (3 arguments)
            _mockNotificationService.Verify(n => n.CreateNotificationAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int?>()), Times.AtLeastOnce);

            // Act: De-escalate
            await _service.EscalateGrievanceAsync(grievance.Id, 101);

            // Assert
            var deescalatedGrievance = await _context.Grievances.FindAsync(grievance.Id);
            Assert.False(deescalatedGrievance.IsEscalated);
        }
    }
}
