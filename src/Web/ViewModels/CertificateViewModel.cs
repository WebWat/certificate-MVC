using ApplicationCore.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class CertificateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "AwardTitleRequired")]
        [Display(Name = "AwardTitle")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "AwardDescriptionRequired")]
        [Display(Name = "AwardDescription")]
        [MaxLength(200)]
        public string Description { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public List<Link> Links { get; set; }

        [Display(Name = "File")]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }

        public byte[] ImageData { get; set; }

        public string UniqueUrl { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Stage")]
        public int Stage { get; set; }
    }
}
