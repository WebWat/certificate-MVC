using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public string Town { get; set; }

        public int Rating { get; set; }

        public byte[] Photo { get; set; }

        public bool OpenData { get; set; }

        public string UniqueUrl { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<Certificate> Certificates { get; set; } = new List<Certificate>();

        public void ClearCertificates() => Certificates.Clear();
    }
}
