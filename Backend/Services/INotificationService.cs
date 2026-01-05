using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DTOs.Notification;

namespace Backend.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetMyNotificationsAsync(int userId);
    Task MarkAsReadAsync(int notificationId, int userId);
    Task MarkAllAsReadAsync(int userId);
    Task CreateNotificationAsync(int userId, string message, int? grievanceId = null);
}
