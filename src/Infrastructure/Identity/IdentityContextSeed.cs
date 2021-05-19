using ApplicationCore.Constants;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (await userManager.FindByNameAsync("admin") is null)
            {
                var admin = new ApplicationUser
                {
                    UserName = AuthorizationConstants.UserName,
                    Email = AuthorizationConstants.EmailAddress,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    UniqueUrl = AuthorizationConstants.UniqueUrl
                };

                await userManager.CreateAsync(admin, AuthorizationConstants.Password);

                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
