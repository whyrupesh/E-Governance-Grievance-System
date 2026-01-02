using Backend.Models;
using Backend.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Backend.Helpers;

namespace Backend.Data
{
    public static class DbSeeder
    {

       
        public static async Task SeedAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();
            Console.WriteLine(">>> DbSeeder started");

            var hasher = new PasswordHasher<User>();

            if (!await context.Users.AnyAsync(u => u.Email == "admin@gov.in"))
            {
                context.Users.Add(new User
                {
                    FullName = "System Admin",
                    Email = "admin@gov.in",
                    Role = UserRole.Admin,
                    PasswordHash = hasher.HashPassword(null!, "Admin@123")
                });
            }

            if (!await context.Users.AnyAsync(u => u.Email == "officer@gov.in"))
            {
                context.Users.Add(new User
                {
                    FullName = "Department Officer",
                    Email = "officer@gov.in",
                    Role = UserRole.Officer,
                    PasswordHash = hasher.HashPassword(null!, "Officer@123")
                });
            }

            if (!await context.Users.AnyAsync(u => u.Email == "supervisor@gov.in"))
            {
                context.Users.Add(new User
                {
                    FullName = "Supervisor",
                    Email = "supervisor@gov.in",
                    Role = UserRole.Supervisor,
                    PasswordHash = hasher.HashPassword(null!, "Supervisor@123")
                });
            }

            await context.SaveChangesAsync();
            Console.WriteLine(">>> Seeding users ended");
        }
    }
}
