using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Identity.Pages.Account.Models;

public class EmailInput
{
    [Required(ErrorMessage = "Required")]
    [EmailAddress(ErrorMessage = "EmailEmail")]
    public string Email { get; set; }
}
