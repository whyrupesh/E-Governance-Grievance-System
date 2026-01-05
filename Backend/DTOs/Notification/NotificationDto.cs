using System;

namespace Backend.DTOs.Notification;

public class NotificationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? RelatedGrievanceId { get; set; }
}
