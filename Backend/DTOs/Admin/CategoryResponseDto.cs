namespace Backend.DTOs.Admin;

public class CategoryResponseDto
{
    public int Id {get; set;}
    public string Name { get; set; } = null!;
    public string DepartmentId { get; set; } = null!;
}
