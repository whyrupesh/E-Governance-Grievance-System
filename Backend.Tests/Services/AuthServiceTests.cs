using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using Backend.DTOs.Auth;
using Backend.Helpers;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Backend.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly JwtTokenHelper _jwtHelper;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("ThisIsASuperSecretKeyForTestingPurposes123!");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
            _mockConfig.Setup(c => c["Jwt:ExpireMinutes"]).Returns("60");

            _jwtHelper = new JwtTokenHelper(_mockConfig.Object);
            _service = new AuthService(_context, _jwtHelper);
        }

        [Fact]
        public async Task Register_ShouldCreateUser_WhenEmailIsUnique()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FullName = "New User",
                Email = "new@user.com",
                Password = "password123"
            };

            // Act
            var result = await _service.RegisterAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new@user.com", result.Email);
            Assert.NotEqual(0, result.UserId);
            
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "new@user.com");
            Assert.NotNull(savedUser);
            Assert.Equal("New User", savedUser.FullName);
        }

        [Fact]
        public async Task Register_ShouldThrowException_WhenEmailExists()
        {
            // Arrange
            _context.Users.Add(new User 
            { 
                FullName = "Existing", 
                Email = "exists@test.com", 
                PasswordHash = "hash" 
            });
            await _context.SaveChangesAsync();

            var dto = new RegisterDto
            {
                FullName = "Another",
                Email = "exists@test.com",
                Password = "pass"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.RegisterAsync(dto));
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            string password = "password123";
            string hash = PasswordHasher.Hash(password);
            
            var user = new User
            {
                 FullName = "Login User",
                 Email = "login@test.com",
                 PasswordHash = hash,
                 Role = Backend.Enums.UserRole.Citizen,
                 IsActive = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Email = "login@test.com",
                Password = password
            };

            // Act
            var result = await _service.LoginAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
        }

        [Fact]
        public async Task Login_ShouldThrowException_WhenCredentialsAreInvalid()
        {
            // Arrange
            var user = new User
            {
                 FullName = "Login User",
                 Email = "valid@test.com",
                 PasswordHash = PasswordHasher.Hash("correct"),
                 Role = Backend.Enums.UserRole.Citizen,
                 IsActive = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Email = "valid@test.com",
                Password = "wrong"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.LoginAsync(dto));
        }
    }
}
