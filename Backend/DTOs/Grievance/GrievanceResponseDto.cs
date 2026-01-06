using Backend.Enums;

namespace Backend.DTOs.Grievance;

public class GrievanceResponseDto
{
    public int Id { get; set; }
    public string GrievanceNumber { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Department { get; set; } = null!;
    public int CitizenId { get; set; }
    public int DepartmentId { get; set; }
    public int CategoryId { get; set; }
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public string Description { get; set; } = "";

    public DateTime? ResolvedAt { get; set; }

    public string ResolutionRemarks { get; set; } = "";

    public string AssignedTo { get; set; } = "";
    public string Feedback { get; set; } = "";
    public int? Rating { get; set; }
    public DateTime? ReopenedAt { get; set; }

    public bool IsEscalated { get; set; }

    public DateTime? EscalatedAt { get; set; }


}
