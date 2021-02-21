using ApplicationCore.Entities;
using System.Collections.Generic;

namespace IntegrationTests.Builders
{
    public class LinkBuilder
    {
        public static List<Link> GetDefaultValues(int certificateId = 0)
        {
            return new List<Link>
            {
                new Link { CertificateId = certificateId, Name = "http://url.certfcate.ru/examplelink1" },
                new Link { CertificateId = certificateId, Name = "http://url.certfcate.ru/examplelinl2" }
            };
        }
    }
}
