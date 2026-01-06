using Xunit;
using Microsoft.EntityFrameworkCore;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Backend.Enums;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Backend.Tests.Services
{
    public class ReportServiceTests
    {
        private readonly AppDbContext _context;
        private readonly ReportService _service;

        public ReportServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new ReportService(_context);
        }

        [Fact]
        public async Task GetGrievanceCountByStatus_ShouldReturnCounts()
        {
            // Arrange
            var dept = new Department { Id = 1, Name = "D", Description = "D" };
            var cat = new Category { Id = 1, Name = "C", DepartmentId = 1 };

            _context.Departments.Add(dept);
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            _context.Grievances.AddRange(
                new Grievance { Id = 1, Status = GrievanceStatus.Submitted, CategoryId = 1, DepartmentId = 1, Description = "D", CitizenId = 1, AssignedTo = "A", Feedback = "", GrievanceNumber = "G1" },
                new Grievance { Id = 2, Status = GrievanceStatus.Submitted, CategoryId = 1, DepartmentId = 1, Description = "D", CitizenId = 1, AssignedTo = "A", Feedback = "", GrievanceNumber = "G2" },
                new Grievance { Id = 3, Status = GrievanceStatus.Resolved, CategoryId = 1, DepartmentId = 1, Description = "D", CitizenId = 1, AssignedTo = "A", Feedback = "", GrievanceNumber = "G3" }
            );

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetGrievanceCountByStatusAsync();

            // Assert
            Assert.Contains(result, r => r.Status == "Submitted" && r.Count == 2);
            Assert.Contains(result, r => r.Status == "Resolved" && r.Count == 1);
        }

        [Fact]
        public async Task GetDepartmentPerformance_ShouldCalculateMetrics()
        {
            // Arrange
            var dept = new Department { Id = 1, Name = "IT", Description = "IT" };
            var cat = new Category { Id = 1, Name = "Hardware", DepartmentId = 1 };

            _context.Departments.Add(dept);
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            _context.Grievances.AddRange(
            new Grievance
            {
                Id = 1,
                DepartmentId = dept.Id,
                Status = GrievanceStatus.Resolved,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                ResolvedAt = DateTime.UtcNow.AddDays(-2),
                GrievanceNumber = "G1",
                CategoryId = 1,
                Description = "D",
                CitizenId = 1,
                AssignedTo = "A",
                Feedback = ""
            },
            new Grievance
            {
                Id = 2,
                DepartmentId = dept.Id,
                Status = GrievanceStatus.Resolved,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                ResolvedAt = DateTime.UtcNow.AddDays(-5),
                GrievanceNumber = "G2",
                CategoryId = 1,
                Description = "D",
                CitizenId = 1,
                AssignedTo = "A",
                Feedback = ""
            },
            new Grievance
            {
                Id = 3,
                DepartmentId = dept.Id,
                Status = GrievanceStatus.Submitted,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                GrievanceNumber = "G3",
                CategoryId = 1,
                Description = "D",
                CitizenId = 1,
                AssignedTo = "A",
                Feedback = ""
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetDepartmentPerformanceAsync();

            // Assert
            var deptStats = result.FirstOrDefault(d => d.Department == "IT");
            Assert.NotNull(deptStats);
            Assert.Equal(4, deptStats.AverageResolutionDays);
            Assert.Equal(2, deptStats.TotalGrievances);
        }

        [Fact]
        public async Task GetGrievanceCountByCategory_ShouldReturnCounts()
        {
            // Arrange
            var dept = new Department { Id = 1, Name = "IT", Description = "IT" };
            var cat1 = new Category { Id = 1, Name = "Hardware", DepartmentId = 1 };
            var cat2 = new Category { Id = 2, Name = "Software", DepartmentId = 1 };

            _context.Departments.Add(dept);
            _context.Categories.AddRange(cat1, cat2);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            _context.Grievances.AddRange(
                new Grievance { Id = 1, CategoryId = 1, DepartmentId = 1, Status = GrievanceStatus.Submitted, Description = "D", CitizenId = 1, AssignedTo = "A", Feedback = "", GrievanceNumber = "G1" },
                new Grievance { Id = 2, CategoryId = 1, DepartmentId = 1, Status = GrievanceStatus.Submitted, Description = "D", CitizenId = 1, AssignedTo = "A", Feedback = "", GrievanceNumber = "G2" },
                new Grievance { Id = 3, CategoryId = 2, DepartmentId = 1, Status = GrievanceStatus.Submitted, Description = "D", CitizenId = 1, AssignedTo = "A", Feedback = "", GrievanceNumber = "G3" }
            );

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetGrievanceCountByCategoryAsync();

            // Assert
            Assert.Contains(result, r => r.Category == "Hardware" && r.Count == 2);
            Assert.Contains(result, r => r.Category == "Software" && r.Count == 1);
        }
    }
}
