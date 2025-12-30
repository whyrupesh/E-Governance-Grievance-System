namespace Backend.DTOs.Admin;

public class CreateOfficerDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int DepartmentId { get; set; }
}
