using System;
using System.ComponentModel.DataAnnotations;
using Backend.Data;
using Backend.DTOs.Auth;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtTokenHelper _jwtHelper;

    public AuthService(AppDbContext context, JwtTokenHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var exists = await _context.Users.AnyAsync( u => u.Email == registerDto.Email);
        if (exists)
        {
            throw new Exception("User Already exists");
        }


        var user = new User
        {
            FullName = registerDto.FullName,
            Email =  registerDto.Email,
            PasswordHash = PasswordHasher.Hash(registerDto.Password),
            Role = Enums.UserRole.Citizen
        };

        _context.Add(user);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email =  user.Email,
            Role = user.Role.ToString()
        };
        
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync( u => u.Email == loginDto.Email && u.IsActive);

        if(user==null || !PasswordHasher.Verify(loginDto.Password, user.PasswordHash))
            throw new Exception("Invalid Credentials");
        

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = _jwtHelper.GenerateToken(user)
        };
    }

}
