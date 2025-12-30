namespace Backend.DTOs.Reports;

public class DepartmentPerformanceDto
{
    public string Department { get; set; } = null!;
    public int TotalGrievances { get; set; }
    public int ResolvedGrievances { get; set; }
    public double AverageResolutionDays { get; set; }
}
