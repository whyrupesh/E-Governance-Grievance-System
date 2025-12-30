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

}
