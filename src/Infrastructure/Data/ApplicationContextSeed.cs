using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationContextSeed
    {
        public static async Task SeedAsync(ApplicationContext context, UserManager<ApplicationUser> userManager,
                                           string imagePath)
        {
            var _user = await userManager.FindByNameAsync(AuthorizationConstants.UserName);

            if (!await context.Certificates.AnyAsync())
            {
                await context.Certificates.AddRangeAsync(GetCertificates(_user.Id, imagePath));
                await context.SaveChangesAsync();
            }

            if (!await context.Links.AnyAsync())
            {
                foreach (var item in await context.Certificates.ToListAsync())
                {
                    await context.Links.AddRangeAsync(GetLinks(item.Id));
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
                                File.ReadAllBytes(path),
                                "2nd place in the Robo-racing category",
                                Stage.AllRussian,
                                DateTime.UtcNow),
                new Certificate(userId,
                                "Robofest",
                                File.ReadAllBytes(path),
                                "3rd place in the RoboFootball category",
                                Stage.AllRussian,
                                DateTime.UtcNow),
            };
        }


        private static IEnumerable<Link> GetLinks(int certificateId)
        {
            return new List<Link>
            {
                new Link ("https://example.com", certificateId),
                new Link ("https://example.com/", certificateId)
            };
        }
    }
}
