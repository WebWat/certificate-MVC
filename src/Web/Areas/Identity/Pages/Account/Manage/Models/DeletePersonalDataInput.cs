using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Identity.Pages.Account.Manage.Models
{
    public class DeletePersonalDataInput
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
