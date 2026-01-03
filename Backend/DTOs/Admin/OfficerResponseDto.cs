using System.Globalization;

namespace Backend.DTOs.Admin;

public class OfficerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Department { get; set; } = null!;

    public string DepartmentId { get; set; } = null!;
    public string Role { get; set; } = "Officer";
}
