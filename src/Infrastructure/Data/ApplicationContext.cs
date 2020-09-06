using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
