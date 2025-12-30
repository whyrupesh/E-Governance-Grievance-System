using Backend.Enums;

namespace Backend.DTOs.Reports;

public class StatusCountDto
{
    public GrievanceStatus Status { get; set; }
    public int Count { get; set; }
}
