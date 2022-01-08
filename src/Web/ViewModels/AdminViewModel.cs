using System;

namespace Web.ViewModels;

public class AdminViewModel
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool EmailConfirmed { get; set; }
}
