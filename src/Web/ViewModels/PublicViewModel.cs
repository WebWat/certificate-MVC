using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class PublicViewModel
    {
        public IEnumerable<CertificateViewModel> Certificates { get; set; }

        public SelectList Years { get; set; }

        [MaxLength(100)]
        public string Find { get; set; }

        public string Year { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string UniqueUrl { get; set; }
        public byte[] ImageData { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}
