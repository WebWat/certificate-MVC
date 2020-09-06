using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationContextSeed
    {
        public static async Task SeedAsync(ApplicationContext context, UserManager<User> userManager)
        {
            var _user = await userManager.FindByNameAsync("admin");
            int certificateId = 0;

            if (!await context.Certificates.AnyAsync() || await context.Certificates.FirstOrDefaultAsync(i => i.Title == "Test") == null)
            {
                await context.Certificates.AddAsync(GetCertificate(_user.Id));
                await context.SaveChangesAsync();
                var certificate = await context.Certificates.FirstOrDefaultAsync(i => i.Title == "Test");
                certificateId = certificate.Id;
            }

            if (!await context.Links.AnyAsync() || await context.Links.FirstOrDefaultAsync(i => i.Name == "Test Link") == null)
            {
                await context.Links.AddRangeAsync(GetLinks(certificateId));
                await context.SaveChangesAsync();
            }

            if (!await context.Events.AnyAsync())
            {
                await context.Events.AddRangeAsync(GetEvents());
                await context.SaveChangesAsync();
            }
        }

        private static Certificate GetCertificate(string userId)
        {
            return new Certificate
            {
                Title = "Test",
                Description = "Test Description",
                Date = DateTime.Now,
                Rating = 2,
                File = new byte[] { 213 },
                UserId = userId
            };
        }

        private static IEnumerable<Link> GetLinks(int id)
        {
            return new List<Link>
            {
                new Link { CertificateId = id, Name = "Test Link" },
                new Link { CertificateId = id, Name = "Test Link 2" }
            };
        }

        private static IEnumerable<Event> GetEvents()
        {
            return new List<Event>
            {
                new Event { Title = "Test Event", Description = "Test Event Description", Date = DateTime.Now, Url = "Test Event Link" },
                new Event { Title = "Test Event 2", Description = "Test Event Description 2", Date = DateTime.Now, Url = "Test Event Link 2" },
            };
        }
    }
}
