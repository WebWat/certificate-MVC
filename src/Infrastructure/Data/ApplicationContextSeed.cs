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
        public static async Task SeedAsync(ApplicationContext context, UserManager<ApplicationUser> userManager, string imagePath)
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

            if (!await context.Events.AnyAsync())
            {
                await context.Events.AddRangeAsync(GetEvents());
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

        private static IEnumerable<Event> GetEvents()
        {
            return new List<Event>
            {
                new Event 
                { 
                    Title = "Selection for November's chemistry programme has started",
                    Description = "At Sirius, the call for applications for the chemistry education programme " +
                                  "in November has started and will take place at the Education Centre from 12 to 30 November 2020.", 
                    Date = DateTime.UtcNow, 
                    Url = "https://sochisirius.ru/news/3829" 
                },
                new Event 
                { 
                    Title = "MIPT invites teachers for courses", 
                    Description = "The Moscow Institute of Physics and Technology (National Research University), within " +
                                  "the framework of the \"Phystech to the Regions\" project, is running a distance-learning course for " +
                                  "physics teachers entitled \"Advanced and Olympiad Preparation of Schoolchildren in Physics\".", 
                    Date = DateTime.UtcNow,
                    Url = "https://mipt.ru/"
                },
            };
        }
    }
}
