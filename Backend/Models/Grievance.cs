using Backend.Enums;

namespace Backend.Models;

public class Grievance
{
    public int Id { get; set; }

    public string GrievanceNumber { get; set; } = null!;

    public int CitizenId { get; set; }
    public User Citizen { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public GrievanceStatus Status { get; set; } = GrievanceStatus.Submitted;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ResolvedAt { get; set; }

    public string? ResolutionRemarks { get; set; }

    public string AssignedTo { get; set; } = null!;
    public string Feedback { get; set; } = null!;
    public int? Rating { get; set; }

    // Escalation
    public bool IsEscalated { get; set; } = false;
    public DateTime? EscalatedAt { get; set; }

    public DateTime? ReopenedAt { get; set; }
}
