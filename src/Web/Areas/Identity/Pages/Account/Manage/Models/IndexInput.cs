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
        [RegularExpression(@"[^&<>\/]*$", ErrorMessage = "RegularExpression")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(100)]
        [Display(Name = "Surname")]
        [RegularExpression(@"[^&<>\/]*$", ErrorMessage = "RegularExpression")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(100)]
        [Display(Name = "MiddleName")]
        [RegularExpression(@"[^&<>\/]*$", ErrorMessage = "RegularExpression")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(100)]
        [Display(Name = "Town")]
        [RegularExpression(@"[^&<>\/]*$", ErrorMessage = "RegularExpression")]
        public string Town { get; set; }

        [Display(Name = "OpenData")]
        public bool OpenData { get; set; }

        [Display(Name = "Photo")]
        public IFormFile File { get; set; }
    }
}
