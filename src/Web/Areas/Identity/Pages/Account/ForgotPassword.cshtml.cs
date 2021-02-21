using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Text;
using System.Threading.Tasks;
using Web.Areas.Identity.Pages.Account.Models;

namespace Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _localizer = localizer;
        }


        [BindProperty]
        public EmailInput Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Page();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code = code },
                        protocol: HttpContext.Request.Scheme);
#if RELEASE
                await _emailSender.SendEmailAsync(Input.Email, _localizer["ForgotEmailSend"], callbackUrl, _localizer["ForgotConfirmSend"]);
                return RedirectToPage("./ForgotPasswordConfirmation");
#elif DEBUG
                return Redirect(callbackUrl);
#endif
            }
            return Page();
        }
    }
}
