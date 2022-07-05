using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Identity.Pages.Account.Models;

public class RegisterInput
{
    [Required(ErrorMessage = "Required")]
    [MaxLength(100)]
    [Display(Name = "Username")]
    [RegularExpression(@"[^&<>\""'/]*$", ErrorMessage = "RegularExpression")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required")]
    [MaxLength(100)]
    [RegularExpression(@"(?i:[аa-яz]*\s[аa-яz]*\s[аa-яz]*)", ErrorMessage = "RegularExpression")]
    [Display(Name = "FullName")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required")]
    [EmailAddress(ErrorMessage = "RegularExpression")]
    [MaxLength(100)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [MaxLength(100)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required")]
    [Compare("Password", ErrorMessage = "PasswordCompare")]
    [DataType(DataType.Password)]
    [MaxLength(100)]
    [Display(Name = "ConfirmPassword")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "OpenData")]
    public bool OpenData { get; set; }
}
