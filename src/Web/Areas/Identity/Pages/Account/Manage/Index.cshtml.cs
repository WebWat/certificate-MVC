using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Web.Extensions;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private readonly long _fileSizeLimit = 2097152;
        private readonly long _fileMinSize = 524288;
        private readonly string _expansion = "image/jpeg";

        private static readonly List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        [Display(Name = "Логин")]
        public string Username { get; set; }

        public SelectList ListClasses = new SelectList(list);

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Это обязательное поле")]
            [MaxLength(100)]
            [Display(Name = "Имя")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Это обязательное поле")]
            [MaxLength(100)]
            [Display(Name = "Фамилия")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Это обязательное поле")]
            [MaxLength(100)]
            [Display(Name = "Отчество")]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "Это обязательное поле")]
            [MaxLength(100)]
            [Display(Name = "Город")]
            public string Country { get; set; }

            [Required(ErrorMessage = "Это обязательное поле")]
            [MaxLength(100)]
            [Display(Name = "Школа")]
            public string School { get; set; }

            [Required(ErrorMessage = "Это обязательное поле")]
            [Display(Name = "Класс")]
            public int Class { get; set; }

            [Display(Name = "Открытые данные")]
            public bool OpenData { get; set; }

            [Display(Name = "Фото")]
            public IFormFile File { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Name = user.Name,
                Surname = user.Surname,
                MiddleName = user.MiddleName,
                Country = user.Country,
                School = user.School,
                Class = user.Class,
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
                    ModelState.AddModelError("Input.File", "Недопустимый формат файла");
                    return Page();
                }

                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(Input.File.OpenReadStream()))
                {

                    if (Input.File.CheckFileSize(_fileMinSize, _fileSizeLimit))
                    {
                        ModelState.AddModelError("Input.File", "Слишком большой размер файла");
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
            user.School = Input.School;
            user.Class = Input.Class;
            user.OpenData = Input.OpenData;

            await _userManager.UpdateAsync(user);

            StatusMessage = "Ваш профиль обновлен";

            return RedirectToPage();
        }
    }
}
