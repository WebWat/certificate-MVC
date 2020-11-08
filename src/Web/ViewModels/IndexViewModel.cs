using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<CertificateViewModel> Certificates { get; set; }

        public SelectList Years { get; set; }

        [MaxLength(100)]
        public string Find { get; set; }

        public string Year { get; set; }
    }
}
