using ApplicationCore.Entities;
using System;

namespace IntegrationTests.Builders
{
    public class CertificateBuilder
    {
        public static Certificate GetDefaultValue()
        {
            return new Certificate
            {
                Title = "Test",
                Description = "Test Description",
                Date = DateTime.Now,
                Rating = 2,
                File = new byte[] { 213 }
            };
        }
    }
}
