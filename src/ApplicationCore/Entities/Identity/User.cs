using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ApplicationCore.Entities.Identity
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
        public string School { get; set; }
        public int Class { get; set; }
        public int Rating { get; set; }
        public byte[] Photo { get; set; }
        public bool OpenData { get; set; }

        public List<Certificate> Certificates { get; set; }

        public string UniqueUrl { get; set; }
    }
}
