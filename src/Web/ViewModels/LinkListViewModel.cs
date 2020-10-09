using System.Collections.Generic;

namespace Web.ViewModels
{
    public class LinkListViewModel
    {
        //Сertificate reference
        public int CertificateId { get; set; }

        //Used for creating
        public LinkViewModel Link { get; set; }

        //Used to display
        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
