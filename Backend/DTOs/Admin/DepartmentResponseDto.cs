namespace Backend.DTOs.Admin;

public class DepartmentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
