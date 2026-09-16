using AppleWorldShoppingMallEMS.Models;
using Microsoft.AspNetCore.Identity;

namespace AppleWorldShoppingMallEMS.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        if (!db.Departments.Any())
        {
            db.Departments.AddRange(
                new Department { Name = "Cleaners", RequiredWorkers = 5, GenderRestriction = "Any" },
                new Department { Name = "Blockers", RequiredWorkers = 5, GenderRestriction = "Male only" },
                new Department { Name = "Organizers", RequiredWorkers = 5, GenderRestriction = "Any" },
                new Department { Name = "Cashiers", RequiredWorkers = 5, GenderRestriction = "Any" },
                new Department { Name = "Security", RequiredWorkers = 10, GenderRestriction = "Any" }
            );
            db.SaveChanges();
        }

        if (!db.Users.Any())
        {
            var hasher = new PasswordHasher<AppUser>();
            var admin = new AppUser
            {
                Username = "admin",
                DisplayName = "Mall Administrator",
                Role = "Admin",
                AccountStatus = "Active"
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");
            db.Users.Add(admin);
            db.SaveChanges();
        }
    }
}