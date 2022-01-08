using ApplicationCore.Constants;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Web.Models;

public class CustomPasswordPolicy : IPasswordValidator<ApplicationUser>
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public CustomPasswordPolicy(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }


    public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
    {
        List<IdentityError> errors = new();

        // Check for admin or user content.
        if (password.ToLower().Contains(Roles.Admin.ToLower()) || 
            password.ToLower().Contains(Roles.User.ToLower()))
        {
            errors.Add(new IdentityError
            {
                Description = _localizer["AdminUserValidation"]
            });
        }

        // Checking for username content.
        if (password.ToLower().Contains(user.UserName.ToLower()))
        {
            errors.Add(new IdentityError
            {
                Description = _localizer["LoginValidation"]
            });
        }

        // Check password length.
        if (password.Length < 9)
        {
            errors.Add(new IdentityError
            {
                Description = _localizer["LengthValidation"]
            });
        }

        Regex regex = new(@"\d");

        // Must contains numbers.
        if (!regex.IsMatch(password))
        {
            errors.Add(new IdentityError
            {
                Description = _localizer["NumbersValidation"]
            });
        }

        regex = new(@"\D");

        // Must contains alphabet.
        if (!regex.IsMatch(password))
        {
            errors.Add(new IdentityError
            {
                Description = _localizer["AlphabetValidation"]
            });
        }

        return Task.FromResult(errors.Count == 0 ?
                               IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
    }
}
