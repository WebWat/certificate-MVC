using Microsoft.AspNetCore.Identity;
using System;

namespace ApplicationCore.Entities.Identity
{
    // TODO: protected?
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public string Town { get; set; }

        public byte[] Photo { get; set; }

        public string UniqueUrl { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
