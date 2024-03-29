﻿using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Identity.Pages.Account.Models;

public class LoginInput
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "UserNameOrEmail")]
    public string UserNameOrEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "RememberMe")]
    public bool RememberMe { get; set; }
}
