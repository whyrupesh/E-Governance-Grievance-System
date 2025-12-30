namespace Backend.DTOs.Admin;

public class CreateCategoryDto
{
    public string Name { get; set; } = null!;
    public int DepartmentId { get; set; }
}
