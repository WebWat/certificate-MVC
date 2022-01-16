namespace ApplicationCore.Entities;

public class Link : BaseEntity
{
    public string Url { get; private set; }
    public string CertificateId { get; private set; }

    public Link(string url, string certificateId)
    {
        Url = url;
        CertificateId = certificateId;
    }
}
