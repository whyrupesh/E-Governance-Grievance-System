using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Services;
using Backend.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Auth Service
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtTokenHelper>();
builder.Services.AddScoped<IGrievanceService,GrievanceService>();
builder.Services.AddScoped<IOfficerService, OfficerService>();
builder.Services.AddScoped<ISupervisorService, SupervisorService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IAdminService, AdminService>();





builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

// SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     await DbSeeder.SeedAsync(dbContext);
// }

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await DbSeeder.ResetPasswordAsync(db, "admin@gov.in", "Admin@123");
    await DbSeeder.ResetPasswordAsync(db, "officer@gov.in", "Officer@123");
    await DbSeeder.ResetPasswordAsync(db, "supervisor@gov.in", "Supervisor@123");
}



app.UseHttpsRedirection();

app.UseCors("AllowAngular");   

app.UseMiddleware<ExceptionMiddleware>();


// JWT comes later
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Run();
