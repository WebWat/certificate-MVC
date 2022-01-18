using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class ApplicationContextSeed
{
    // Not tested.
    public static async Task SeedAsync(ApplicationContext context, 
                                       UserManager<ApplicationUser> userManager,
                                       string imagePath)
    {
        // Get the user to get his Id.
        var _user = await userManager.FindByNameAsync(AuthorizationConstants.UserName);

        // Filling out certificates.
        if ((await context.Certificates.AsNoTracking().ToListAsync()).Count == 0)
        {
            await context.Certificates.AddRangeAsync(GetCertificates(_user.Id, imagePath));
            await context.SaveChangesAsync();
        }

        // Filling out links.
        if ((await context.Links.AsNoTracking().ToListAsync()).Count == 0)
        {
            foreach (var item in await context.Certificates.ToListAsync())
            {
                await context.Links.AddRangeAsync(GetLinks(item.Id));
                break;
            }

            await context.SaveChangesAsync();
        }
    }


    private static IEnumerable<Certificate> GetCertificates(string userId, string path)
    {
        return new List<Certificate>
        {
            new Certificate(userId,
                            "Robofest",
                            path,
                            "2nd place in the Robo-racing category",
                            Stage.AllRussian,
                            DateTime.UtcNow),
            new Certificate(userId,
                            "Robofest",
                            path,
                            "3rd place in the RoboFootball category",
                            Stage.AllRussian,
                            DateTime.UtcNow),
        };
    }


    private static IEnumerable<Link> GetLinks(string certificateId)
    {
        return new List<Link>
        {
            new Link("https://example.com", certificateId),
            new Link("https://example.com/", certificateId)
        };
    }
}

