using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using Web;
using Web.Models;
using Web.Areas.Identity.Pages.Account.Models;
using Web.Interfaces;

namespace Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUrlGenerator _urlGenerator;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailTemplate _emailTemplate;

        public RegisterModel(UserManager<ApplicationUser> userManager, 
                             IEmailSender emailSender, 
                             IUrlGenerator urlGenerator, 
                             IStringLocalizer<SharedResource> localizer,
                             ILogger<RegisterModel> logger,
                             IEmailTemplate emailTemplate)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _urlGenerator = urlGenerator;
            _localizer = localizer;
            _logger = logger;
            _emailTemplate = emailTemplate;
        }


        [BindProperty]
        public RegisterInput Input { get; set; }

        public string ReturnUrl { get; set; }


        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var _user = await _userManager.FindByNameAsync(Input.UserName);

                if (_user != null)
                {
                    ModelState.AddModelError("Input.UserName", _localizer["LoginExist"]);
                    return Page();
                }

                _user = await _userManager.FindByEmailAsync(Input.Email);

                if (_user != null)
                {
                    ModelState.AddModelError(string.Empty, _localizer["EmailExist"]);
                    return Page();
                }

                string[] fullName = Input.FullName.Split(' ');

                ApplicationUser user = new ApplicationUser
                {
                    Email = Input.Email,
                    UserName = Input.UserName,
                    Name = fullName[1],
                    Surname = fullName[0],
                    MiddleName = fullName[2],
                    UniqueUrl = _urlGenerator.Generate(),
                    RegistrationDate = DateTime.UtcNow          
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"New User {user.Id} registered");

                    await _userManager.AddToRoleAsync(user, "User");
#if DEBUG
                    return RedirectToPage("./Login");
#elif RELEASE
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page("/Account/ConfirmEmail",
                                               pageHandler: null,
                                               values: new { area = "Identity", userId = user.Id, code = code },
                                               protocol: Request.Scheme);

                    var email = _emailTemplate.GetTemplate(EmailMessageType.RegisterConfirmation, callbackUrl);

                    await _emailSender.SendEmailAsync(Input.Email, email.subject, email.template);

                    return RedirectToPage("./RegisterConfirmation");
#endif
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
