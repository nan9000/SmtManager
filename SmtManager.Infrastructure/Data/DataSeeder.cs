using SmtManager.Application.Helpers;
using SmtManager.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmtManager.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(SmtDbContext context)
    {
        if (!await context.Users.AnyAsync(u => u.Username == "admin"))
        {
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@smtmanager.com",
                PasswordHash = PasswordHasher.HashPassword("admin123"),
                Role = "Admin"
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
