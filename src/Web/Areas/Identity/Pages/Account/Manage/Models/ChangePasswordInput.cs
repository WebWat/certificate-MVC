using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Identity.Pages.Account.Manage.Models;

public class ChangePasswordInput
{
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [MaxLength(100)]
    [Display(Name = "OldPassword")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [MaxLength(100)]
    [Display(Name = "NewPassword")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Required")]
    [Compare("NewPassword", ErrorMessage = "PasswordCompare")]
    [DataType(DataType.Password)]
    [MaxLength(100)]
    [Display(Name = "ConfirmPassword")]
    public string ConfirmPassword { get; set; }
}

