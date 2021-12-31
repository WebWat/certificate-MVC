using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Web.Areas.Identity.Pages.Account.Manage;

public class ChangeUrlModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUrlGenerator _urlGenerator;
    private readonly IStringLocalizer<ChangeUrl> _localizer;
    private readonly ILogger<ChangeUrlModel> _logger;

    public ChangeUrlModel(UserManager<ApplicationUser> userManager,
                          IUrlGenerator urlGenerator,
                          IStringLocalizer<ChangeUrl> localizer,
                          ILogger<ChangeUrlModel> logger)
    {
        _userManager = userManager;
        _urlGenerator = urlGenerator;
        _localizer = localizer;
        _logger = logger;
    }


    [TempData]
    public string StatusMessage { get; set; }


    public async Task<IActionResult> OnPostAsync()
    {
        var _user = await _userManager.GetUserAsync(User);

        _user.UniqueUrl = _urlGenerator.Generate();

        await _userManager.UpdateAsync(_user);

        _logger.LogInformation($"User {_user.Id} has changed his url");

        StatusMessage = _localizer["StatusMessage"];

        return Page();
    }
}

public class ChangeUrl { }
