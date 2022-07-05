using Microsoft.AspNetCore.Identity;
using System;

namespace ApplicationCore.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string MiddleName { get; set; } = string.Empty;

    public string? Town { get; set; }

    public byte[]? Photo { get; set; }

    public string UniqueUrl { get; set; } = string.Empty;

    public DateTime RegistrationDate { get; set; }
}
