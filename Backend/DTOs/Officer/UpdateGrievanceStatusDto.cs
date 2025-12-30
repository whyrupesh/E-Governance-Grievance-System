using Backend.Enums;

namespace Backend.DTOs.Officer;

public class UpdateGrievanceStatusDto
{
    public GrievanceStatus Status { get; set; }
    public string? ResolutionRemarks { get; set; }
}
