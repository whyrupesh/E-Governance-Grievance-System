namespace Backend.DTOs.Grievance;

public class CreateGrievanceDto
{
    public int CategoryId { get; set; }
    public string Description { get; set; } = null!;
}
