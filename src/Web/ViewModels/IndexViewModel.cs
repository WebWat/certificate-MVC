using System.Collections.Generic;

namespace Web.ViewModels;

public class IndexViewModel : SearchViewModel
{
    public IEnumerable<CertificateViewModel> Certificates { get; set; }

    public PageViewModel PageViewModel { get; set; }
}