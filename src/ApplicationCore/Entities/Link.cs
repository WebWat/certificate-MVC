namespace ApplicationCore.Entities;

public class Link : BaseEntity
{
    public string Url { get; private set; }
    public int CertificateId { get; private set; }

    public Link(string url, int certificateId)
    {
        Url = url;
        CertificateId = certificateId;
    }

    public Link SetId(int id)
    {
        Id = id;
        return this;
    }
}
