namespace ApplicationCore.Entities
{
    public class Link : BaseEntity
    {
        public string Name { get; set; }

        public int CertificateId { get; set; }

        public Certificate Certificate { get; set; }
    }
}
