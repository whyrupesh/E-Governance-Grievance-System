using Xunit;
using Microsoft.EntityFrameworkCore;
using Backend.Services;
using Backend.Data;
using Backend.Models;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Backend.Tests.Services
{
    public class NotificationServiceTests
    {
        private readonly AppDbContext _context;
        private readonly NotificationService _service;

        public NotificationServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new NotificationService(_context);
        }

        [Fact]
        public async Task CreateNotification_ShouldAddNotification()
        {
            // Act
            await _service.CreateNotificationAsync(1, "Test Message", 100);

            // Assert
            var notification = await _context.Notifications.FirstOrDefaultAsync();
            Assert.NotNull(notification);
            Assert.Equal("Test Message", notification.Message);
            Assert.Equal(1, notification.UserId);
            Assert.False(notification.IsRead);
            Assert.Equal(100, notification.RelatedGrievanceId);
        }

        [Fact]
        public async Task GetMyNotifications_ShouldReturnOnlyUserNotifications()
        {
            // Arrange
            _context.Notifications.Add(new Notification { UserId = 1, Message = "Msg 1", CreatedAt = DateTime.UtcNow.AddMinutes(-10) });
            _context.Notifications.Add(new Notification { UserId = 2, Message = "Msg 2" }); // Other user
            _context.Notifications.Add(new Notification { UserId = 1, Message = "Msg 3", CreatedAt = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetMyNotificationsAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Msg 3", result.First().Message); // Ordered by Descending
        }

        [Fact]
        public async Task MarkAsRead_ShouldUpdateStatus()
        {
            // Arrange
            var notification = new Notification { UserId = 1, Message = "Unread", IsRead = false };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Act
            await _service.MarkAsReadAsync(notification.Id, 1);

            // Assert
            var updated = await _context.Notifications.FindAsync(notification.Id);
            Assert.True(updated.IsRead);
        }

        [Fact]
        public async Task MarkAllAsRead_ShouldUpdateAllUserNotifications()
        {
            // Arrange
            _context.Notifications.Add(new Notification { UserId = 1, Message = "1", IsRead = false });
            _context.Notifications.Add(new Notification { UserId = 1, Message = "2", IsRead = false });
            _context.Notifications.Add(new Notification { UserId = 1, Message = "3", IsRead = true });
            _context.Notifications.Add(new Notification { UserId = 2, Message = "4", IsRead = false }); // Other user
            await _context.SaveChangesAsync();

            // Act
            await _service.MarkAllAsReadAsync(1);

            // Assert
            var userNotifications = await _context.Notifications.Where(n => n.UserId == 1).ToListAsync();
            Assert.All(userNotifications, n => Assert.True(n.IsRead));
            
            var otherUserNotification = await _context.Notifications.FirstOrDefaultAsync(n => n.UserId == 2);
            Assert.False(otherUserNotification.IsRead);
        }
    }
}
