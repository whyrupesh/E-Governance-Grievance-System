using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    //later we will create tables and all
    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Grievance> Grievances => Set<Grievance>();


    //Prevents duplicate registrations.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();


        // Department seed
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Water Supply", Description = "Water related services" },
            new Department { Id = 2, Name = "Electricity", Description = "Electricity related services" },
            new Department { Id = 3, Name = "Municipal Services", Description = "Municipal services" }
        );

        // Category seed
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "No Water Supply", DepartmentId = 1 },
            new Category { Id = 2, Name = "Water Leakage", DepartmentId = 1 },
            new Category { Id = 3, Name = "Power Cut", DepartmentId = 2 },
            new Category { Id = 4, Name = "Street Light Not Working", DepartmentId = 2 },
            new Category { Id = 5, Name = "Garbage Not Collected", DepartmentId = 3 }
        );

        modelBuilder.Entity<Category>()
            .HasOne(c => c.Department)
            .WithMany()
            .HasForeignKey(c => c.DepartmentId);

        modelBuilder.Entity<Grievance>()
            .HasOne(g => g.Citizen)
            .WithMany()
            .HasForeignKey(g => g.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Grievance>()
            .HasOne(g => g.Category)
            .WithMany()
            .HasForeignKey(g => g.CategoryId);

        modelBuilder.Entity<Grievance>()
            .HasOne(g => g.Department)
            .WithMany()
            .HasForeignKey(g => g.DepartmentId);

    }

}
