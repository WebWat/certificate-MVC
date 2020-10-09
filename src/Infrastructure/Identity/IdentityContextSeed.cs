using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
                await roleManager.CreateAsync(new IdentityRole("Moderator"));
            }

            if (await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            if (await userManager.FindByNameAsync("admin") == null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@a",
                    OpenData = true,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    UniqueUrl = "NTE4ZjE2NWMtMTMwOS00MmYwLWFmZTUtMGMyZTBjNmU5NzY5MTkwNzMzYmEtMDE5MS00ZWFkLTlkOWQtZGQ3M2RiMDkxZjMy"
                };

                await userManager.CreateAsync(admin, "Password12");

                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
