using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Web.Areas.Identity.Pages.Account.Manage
{   
    public class ChangeUrlModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IUrlGenerator _urlGenerator;
        private readonly IStringLocalizer<ChangeUrl> _localizer;

        public ChangeUrlModel(UserManager<User> userManager, IUrlGenerator urlGenerator, IStringLocalizer<ChangeUrl> localizer)
        {
            _userManager = userManager;
            _urlGenerator = urlGenerator;
            _localizer = localizer;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var _user = await _userManager.GetUserAsync(User);

            _user.UniqueUrl = _urlGenerator.Generate();

            await _userManager.UpdateAsync(_user);

            StatusMessage = _localizer["StatusMessage"];

            return Page();
        }
    }

    public class ChangeUrl { }
}
