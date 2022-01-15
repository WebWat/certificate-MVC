using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;

namespace ApplicationCore.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    //[JsonProperty(PropertyName = "partition")]
    //public string Partition { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string MiddleName { get; set; }

    public string Town { get; set; }

    public byte[] Photo { get; set; }

    public string UniqueUrl { get; set; }

    public DateTime RegistrationDate { get; set; }
}
