using System.Collections.Generic;

namespace Web.ViewModels;

public class PublicViewModel : SearchViewModel
{
    public IEnumerable<CertificateViewModel> Certificates { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string UniqueUrl { get; set; } = string.Empty;
    public byte[]? ImageData { get; set; }

    public PageViewModel PageViewModel { get; set; } = default!;
}