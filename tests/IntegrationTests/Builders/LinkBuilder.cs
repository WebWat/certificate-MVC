using ApplicationCore.Entities;
using System.Collections.Generic;

namespace IntegrationTests.Builders
{
    public class LinkBuilder
    {
        public static List<Link> GetDefaultValues()
        {
            return new List<Link>
            {
                new Link { CertificateId = 1, Name = "Test Link" },
                new Link { CertificateId = 1, Name = "Test Link 2" }
            };
        }
    }
}
