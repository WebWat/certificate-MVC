using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Web.Areas.Identity.Pages.Account.Manage.Models;

namespace Web.Areas.Identity.Pages.Account.Manage;

[Authorize]
public class DeletePersonalDataModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IStringLocalizer<DeletePersonalData> _localizer;
    private readonly ILogger<DeletePersonalDataModel> _logger;
    private readonly ICertificateRepository _repository;

    public DeletePersonalDataModel(UserManager<ApplicationUser> userManager,
                                   SignInManager<ApplicationUser> signInManager,
                                   IStringLocalizer<DeletePersonalData> localizer,
                                   ILogger<DeletePersonalDataModel> logger,
                                   ICertificateRepository repository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _localizer = localizer;
        _logger = logger;
        _repository = repository;
    }


    [BindProperty]
    public DeletePersonalDataInput Input { get; set; }

    public bool RequirePassword { get; set; }


    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);

        RequirePassword = await _userManager.HasPasswordAsync(user);

        return Page();
    }


    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(User);

        RequirePassword = await _userManager.HasPasswordAsync(user);

        if (RequirePassword)
        {
            // If the password is incorrect.
            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, _localizer["Error"]);
                return Page();
            }
        }

        await _repository.DeleteCertificatesByUserId(user.Id, cancellationToken);

        await _userManager.DeleteAsync(user);

        _logger.LogInformation($"User {user.Id} has been deleted");

        await _signInManager.SignOutAsync();

        return Redirect("~/");
    }
}

public class DeletePersonalData { }
