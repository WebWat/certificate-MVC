using ApplicationCore.Entities;

namespace IntegrationTests.Builders
{
    public class CertificateBuilder
    {
        public static Certificate GetDefaultValue()
        {
            return new Certificate
            {
                Title = "Robofest"
            };
        }
    }
}
