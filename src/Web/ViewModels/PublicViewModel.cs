using System.Collections.Generic;

namespace Web.ViewModels;

public class PublicViewModel : SearchViewModel
{
    public IEnumerable<CertificateViewModel> Certificates { get; set; }

    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string Surname { get; set; }
    public string UniqueUrl { get; set; }
    public byte[] ImageData { get; set; }

    public PageViewModel PageViewModel { get; set; }
}