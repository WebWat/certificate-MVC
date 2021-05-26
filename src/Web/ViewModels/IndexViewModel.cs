using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<CertificateViewModel> Certificates { get; set; }

        public SelectList Stages { get; set; }

        public SelectList Years { get; set; }

        [MaxLength(100)]
        public string Find { get; set; }

        public string Year { get; set; }

        public Stage? Stage { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}
