using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Identity.Pages.Account.Models;

public class ResetInput
{
    [Required(ErrorMessage = "Required")]
    [EmailAddress(ErrorMessage = "RegularExpression")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [MaxLength(40)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Required")]
    [Compare("Password", ErrorMessage = "PasswordCompare")]
    [DataType(DataType.Password)]
    [MaxLength(40)]
    [Display(Name = "ConfirmPassword")]
    public string ConfirmPassword { get; set; }

    public string Code { get; set; }
}
