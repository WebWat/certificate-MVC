using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading.Tasks;
using Web.Areas.Identity.Pages.Account.Models;
using Web.Interfaces;
using Web.Models;

namespace Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly IEmailTemplate _emailTemplate;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager,
                               IEmailSender emailSender,
                               IEmailTemplate emailTemplate)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _emailTemplate = emailTemplate;
    }


    [BindProperty]
    public EmailInput Input { get; set; } = new();


    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return Page();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string? callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code }, // TODO: ?.
                    protocol: HttpContext.Request.Scheme);

            if (callbackUrl is null)
            {
                return BadRequest();
            }

// Remove the mandatory check by mail during debugging.
#if RELEASE
                var email = _emailTemplate.GetTemplate(EmailMessageType.ForgotPasswordConfirmation, callbackUrl);

                await _emailSender.SendEmailAsync(Input.Email, email.subject, email.template);
                return RedirectToPage("./ForgotPasswordConfirmation");
#elif DEBUG
            return Redirect(callbackUrl);
#endif
        }
        return Page();
    }
}
