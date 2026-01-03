namespace Backend.DTOs.Admin;

public class CategoryResponseDto
{
    public int Id {get; set;}
    public string Name { get; set; } = null!;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
}
