using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ChangePasswordModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Это обязательное поле")]
            [DataType(DataType.Password)]
            [MaxLength(100)]
            [Display(Name = "Старый пароль")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "Это обязательное поле")]
            [DataType(DataType.Password)]
            [MaxLength(100)]
            [Display(Name = "Новый пароль")]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Это обязательное поле")]
            [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
            [DataType(DataType.Password)]
            [MaxLength(100)]
            [Display(Name = "Подтвердить пароль")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (!hasPassword)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Ваш пароль был изменен";

            return RedirectToPage();
        }
    }
}
