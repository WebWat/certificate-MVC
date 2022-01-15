using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PieroDeTomi.EntityFrameworkCore.Identity.Cosmos;
using System;


namespace Infrastructure.Data;

public class ApplicationContext : CosmosIdentityDbContext<ApplicationUser>
{
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Link> Links { get; set; }

    public ApplicationContext(DbContextOptions dbContextOptions, IOptions<OperationalStoreOptions> options)
        : base(dbContextOptions, options)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultContainer("Container1");

        // Convert the Stage property of the Certificate entity
        // to store enumeration values as a string.
        modelBuilder.Entity<Certificate>()
                    .Property(e => e.Stage)
                    .HasConversion(v => v.ToString(), v => (Stage)Enum.Parse(typeof(Stage), v));


        base.OnModelCreating(modelBuilder);
    }
}
