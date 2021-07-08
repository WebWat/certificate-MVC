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
            var _user = await userManager.FindByNameAsync("admin");

            if (!await context.Certificates.AnyAsync())
            {
                await context.Certificates.AddRangeAsync(GetCertificates(_user.Id, imagePath));
                await context.SaveChangesAsync();
            }

            if (!await context.Links.AnyAsync())
            {
                foreach (var item in await context.Certificates.ToListAsync())
                {
                    await context.Links.AddRangeAsync(GetLinks(_user.Id, item.Id));
                }
                await context.SaveChangesAsync();
            }
        }


        private static IEnumerable<Certificate> GetCertificates(string userId, string path)
        {
            return new List<Certificate>
            {
                new Certificate
                {
                    Title = "Robofest",
                    Description = "2nd place in the Robo-racing category",
                    Date = DateTime.UtcNow,
                    Stage = Stage.AllRussian,
                    File = File.ReadAllBytes(path),
                    UserId = userId
                },
                new Certificate
                {
                    Title = "Robofest",
                    Description = "3rd place in the RoboFootball category",
                    Date = DateTime.UtcNow,
                    Stage = Stage.AllRussian,
                    File = File.ReadAllBytes(path),
                    UserId = userId
                }
            };
        }


        private static IEnumerable<Link> GetLinks(string userId, int certificateId)
        {
            return new List<Link>
            {
                new Link { CertificateId = certificateId, Name = "http://url.certfcate.ru/examplelink1", UserId = userId },
                new Link { CertificateId = certificateId, Name = "http://url.certfcate.ru/examplelink2", UserId = userId }
            };
        }
    }
}
