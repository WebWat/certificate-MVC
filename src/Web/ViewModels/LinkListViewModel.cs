using System.Collections.Generic;

namespace Web.ViewModels
{
    public class LinkListViewModel
    {
        public int CertificateId { get; set; }

        public LinkViewModel Link { get; set; }

        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
