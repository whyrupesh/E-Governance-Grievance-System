using System;

namespace Backend.Models;

public class Notification : BaseEntity
{
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public int? RelatedGrievanceId { get; set; }

    // Navigation props
    public User? User { get; set; }
}
