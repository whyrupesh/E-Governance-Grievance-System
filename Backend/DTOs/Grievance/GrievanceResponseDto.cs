using Backend.Enums;

namespace Backend.DTOs.Grievance;

public class GrievanceResponseDto
{
    public int Id { get; set; }
    public string GrievanceNumber { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Department { get; set; } = null!;
    public GrievanceStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
