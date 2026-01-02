using System;
using Backend.Enums;

namespace Backend.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public UserRole Role { get; set; }

    public bool IsActive { get; set; } = true;


    // Officer only
    public int? DepartmentId { get; set; } //nullable because admins and citizen donot belongs to any department

}
