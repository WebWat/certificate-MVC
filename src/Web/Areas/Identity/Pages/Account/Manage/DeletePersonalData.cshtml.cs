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

namespace Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class DeletePersonalDataModel : PageModel
    {
        private readonly IUserRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<DeletePersonalData> _localizer;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(UserManager<ApplicationUser> userManager,
                                       SignInManager<ApplicationUser> signInManager,
                                       IUserRepository repository,
                                       IStringLocalizer<DeletePersonalData> localizer,
                                       ILogger<DeletePersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
            _localizer = localizer;
            _logger = logger;
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
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, _localizer["Error"]);
                    return Page();
                }
            }

            await _repository.DeleteUserAsync(user.Id, cancellationToken);

            _logger.LogInformation($"User {user.Id} has been deleted");

            await _signInManager.SignOutAsync();

            return Redirect("~/");
        }
    }

    public class DeletePersonalData { }
}
