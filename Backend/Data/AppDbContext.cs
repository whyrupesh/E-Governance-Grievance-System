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
    public DbSet<Notification> Notifications => Set<Notification>();


    //Prevents duplicate registrations.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

        //Relationships
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
