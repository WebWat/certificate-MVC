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
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
                                       IRepository _repo)
    {
        // Filling roles.
        //if (await roleManager.FindByNameAsync(Roles.Admin, source.Token) is null)
        //{
        //    await roleManager.CreateAsync(new IdentityRole(Roles.Admin), default);
        //    await roleManager.CreateAsync(new IdentityRole(Roles.User), default);
        //}
        var admin2 = await userManager.FindByNameAsync(AuthorizationConstants.UserName);
        var roleIds = await _repo
                .Table<IdentityUserRole<string>>()
                .Where(m => m.UserId == admin2.Id)
                .Select(m => m.RoleId)
                .ToListAsync();

        IList<string> res = await _repo
            .Table<IdentityRole>()
            .Where(m => roleIds.Contains(m.Id))
            .Select(m => m.Name)
            .ToListAsync();
        //var role = await _repo.Table<IdentityRole>()
        //        .SingleOrDefaultAsync(_ => _.Name == Roles.Admin);



        //IdentityUserRole<string> userRole = new IdentityUserRole<string>
        //{
        //    RoleId = role.Id,
        //    UserId = admin2?.Id,
        //};

        //_repo.Add(userRole);
        //await _repo.SaveChangesAsync();

        // Creating a user.
        //if (await userManager.FindByNameAsync(AuthorizationConstants.UserName) is null)
        //{
        //    var admin = new ApplicationUser
        //    {
        //        UserName = AuthorizationConstants.UserName,
        //        Email = AuthorizationConstants.EmailAddress,
        //        EmailConfirmed = true,
        //        LockoutEnabled = false,
        //        UniqueUrl = AuthorizationConstants.UniqueUrl
        //    };

        //    await userManager.CreateAsync(admin, AuthorizationConstants.Password);

        //    await userManager.AddToRoleAsync(admin, Roles.Admin);
        //}
    }
}
