using ApplicationCore.Constants;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PieroDeTomi.EntityFrameworkCore.Identity.Cosmos.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Identity;

public class IdentityContextSeed
{
    // Not tested.
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
                                       IRepository _repo)
    {
        var role = await _repo.Table<IdentityRole>()
                .SingleOrDefaultAsync(_ => _.Name == Roles.Admin);

        // Add roles.
        if (role is null)
        {
            _repo.Add(new IdentityRole(Roles.Admin));
            await _repo.SaveChangesAsync();
            _repo.Add(new IdentityRole(Roles.User));
            await _repo.SaveChangesAsync();
        }

        // Add user.
        if (await userManager.FindByNameAsync(AuthorizationConstants.UserName) is null)
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

            role = await _repo.Table<IdentityRole>()
                .SingleOrDefaultAsync(_ => _.Name == Roles.Admin);

            IdentityUserRole<string> userRole = new IdentityUserRole<string>
            {
                RoleId = role.Id,
                UserId = admin.Id,
            };

            _repo.Add(userRole);
            await _repo.SaveChangesAsync();
        }
    }
}
