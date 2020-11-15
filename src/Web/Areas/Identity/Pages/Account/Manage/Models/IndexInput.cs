using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Identity.Pages.Account.Manage.Models
{
    public class IndexInput
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(100)]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(100)]
        [Display(Name = "MiddleName")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(100)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "OpenData")]
        public bool OpenData { get; set; }

        [Display(Name = "Photo")]
        public IFormFile File { get; set; }
    }
}
