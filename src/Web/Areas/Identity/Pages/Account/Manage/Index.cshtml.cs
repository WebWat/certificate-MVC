using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Web.Areas.Identity.Pages.Account.Manage.Models;
using Web.Extensions;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class IndexModel : PageModel
    {      
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<Index> _localizer;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(UserManager<ApplicationUser> userManager, 
                          IStringLocalizer<Index> localizer, 
                          ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _localizer = localizer;
            _logger = logger;
        }

        private readonly long _fileSizeLimit = 2097152;
        private readonly long _fileMinSize = 524288;
        private readonly string _expansion = "image/jpeg";

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IndexInput Input { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Input = new IndexInput
            {
                Username = userName,
                Name = user.Name,
                Surname = user.Surname,
                MiddleName = user.MiddleName,
                Town = user.Town,
                OpenData = user.OpenData
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

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
            user.Town = Input.Town;
            user.OpenData = Input.OpenData;

            await _userManager.UpdateAsync(user);

            _logger.LogInformation($"User {user.Id} updated his profile");

            StatusMessage = _localizer["StatusMessage"];

            return RedirectToPage();
        }
    }

    public class Index { }
}
