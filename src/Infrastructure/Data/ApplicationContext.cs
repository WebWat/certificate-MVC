using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Data;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Certificate> Certificates { get; set; } = default!;
    public DbSet<Link> Links { get; set; } = default!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Convert the Stage property of the Certificate entity
        // to store enumeration values as a string.
        modelBuilder.Entity<Certificate>()
                        .Property(e => e.Stage)
                        .HasConversion(v => v.ToString(), v => (Stage)Enum.Parse(typeof(Stage), v));

        base.OnModelCreating(modelBuilder);
    }
}