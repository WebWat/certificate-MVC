using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.IO;
using System.Threading.Tasks;
using Web.Areas.Identity.Pages.Account.Manage.Models;
using Web.Extensions;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class IndexModel : PageModel
    {      
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<Index> _localizer;

        public IndexModel(UserManager<User> userManager, IStringLocalizer<Index> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        private readonly long _fileSizeLimit = 2097152;
        private readonly long _fileMinSize = 524288;
        private readonly string _expansion = "image/jpeg";

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IndexInput Input { get; set; }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Input = new IndexInput
            {
                Username = userName,
                Name = user.Name,
                Surname = user.Surname,
                MiddleName = user.MiddleName,
                Country = user.Country,
                OpenData = user.OpenData
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.File != null)
            {
                if (Input.File.CheckFileExtension(_expansion))
                {
                    ModelState.AddModelError("Input.File", _localizer["FileExtensionError"]);
                    return Page();
                }

                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(Input.File.OpenReadStream()))
                {

                    if (Input.File.CheckFileSize(_fileMinSize, _fileSizeLimit))
                    {
                        ModelState.AddModelError("Input.File", _localizer["FileSizeError"]);
                        return Page();
                    }
                    imageData = binaryReader.ReadBytes((int)Input.File.Length);
                }

                user.Photo = imageData;
            }

            user.Surname = Input.Surname;
            user.Name = Input.Name;
            user.MiddleName = Input.MiddleName;
            user.Country = Input.Country;
            user.OpenData = Input.OpenData;

            await _userManager.UpdateAsync(user);

            StatusMessage = _localizer["StatusMessage"];

            return RedirectToPage();
        }
    }

    public class Index { }
}
