using Backend.Enums;

namespace Backend.DTOs.Reports;

public class StatusCountDto
{
    public string Status { get; set; } = null!;
    public int Count { get; set; }
}
