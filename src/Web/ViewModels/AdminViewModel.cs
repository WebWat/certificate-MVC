using System;

namespace Web.ViewModels;

public class AdminViewModel
{
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public bool EmailConfirmed { get; set; }
}
