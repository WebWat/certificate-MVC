using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Web.Areas.Identity.Pages.Account.Models;

namespace Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUrlGenerator _urlGenerator;

        public RegisterModel(UserManager<User> userManager, IEmailSender emailSender, IUrlGenerator urlGenerator)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _urlGenerator = urlGenerator;
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
                    ModelState.AddModelError("Input.UserName", "Такой логин уже существует");
                    return Page();
                }

                _user = await _userManager.FindByEmailAsync(Input.Email);

                if (_user != null)
                {
                    ModelState.AddModelError(string.Empty, "Данный email уже имеется");
                    return Page();
                }

                string[] fullName = Input.FullName.Split(' ');

                User user = new User
                {
                    Email = Input.Email,
                    UserName = Input.UserName,
                    Name = fullName[1],
                    Surname = fullName[0],
                    MiddleName = fullName[2],
                    UniqueUrl = _urlGenerator.Generate(),
                    OpenData = Input.OpenData
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
#if DEBUG
                    return RedirectToPage("./Login");
#elif RELEASE
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Подтвердите вашу учетную запись", callbackUrl, "Подтвердить");

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
