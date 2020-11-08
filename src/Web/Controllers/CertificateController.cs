using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class CertificateController : Controller
    {
        private readonly ICertificateViewModelService _certificateService;
        private readonly UserManager<User> _userManager;
        private readonly long _fileSizeLimit = 2097152;
        private readonly long _fileMinSize = 524288;
        private readonly string _expansion = "image/jpeg";

        public CertificateController(ICertificateViewModelService certificateService, UserManager<User> userManager)
        {
            _certificateService = certificateService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string year, string find)
        {
            var _user = await _userManager.GetUserAsync(User);

            return View(_certificateService.GetIndexViewModel(_user.Id, year, find));
        }

        public async Task<IActionResult> Details(int id)
        {
            var _user = await _userManager.GetUserAsync(User);

            var certificate = await _certificateService.GetCertificateByIdIncludeLinksAsync(id, _user.Id);

            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CertificateViewModel cvm)
        {
            var _user = await _userManager.GetUserAsync(User);

            if (cvm.File == null)
            {
                ModelState.AddModelError("File", "Это обязательное поле");
            }

            if (ModelState.IsValid)
            {
                if (cvm.File != null)
                {
                    if (cvm.File.CheckFileExtension(_expansion)) 
                    {
                        ModelState.AddModelError("File", "Недопустимый формат файла");
                        return View();
                    }

                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(cvm.File.OpenReadStream()))
                    {

                        if (cvm.File.CheckFileSize(_fileMinSize, _fileSizeLimit))
                        {
                            ModelState.AddModelError("File", "Недопустимый размер файла");
                            return View();
                        }
                        imageData = binaryReader.ReadBytes((int)cvm.File.Length);
                    }

                    cvm.ImageData = imageData;
                }

                await _certificateService.CreateCertificateAsync(cvm, _user.Id);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var _user = await _userManager.GetUserAsync(User);

            var certificate = await _certificateService.GetCertificateByIdAsync(id, _user.Id);

            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CertificateViewModel cvm)
        {
            var _user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                if (cvm.File != null)
                {
                    if (cvm.File.CheckFileExtension(_expansion))
                    {
                        ModelState.AddModelError("File", "Недопустимый формат файла");
                        return View(cvm);
                    }

                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(cvm.File.OpenReadStream()))
                    {
                        if (cvm.File.CheckFileSize(_fileMinSize, _fileSizeLimit))
                        {
                            ModelState.AddModelError("File", "Недопустимый размер файла");
                            return View(cvm);
                        }
                        imageData = binaryReader.ReadBytes((int)cvm.File.Length);
                    }

                    cvm.ImageData = imageData;
                }

                await _certificateService.UpdateCertificateAsync(cvm, _user.Id);

                return RedirectToAction("Details", new { id = cvm.Id });
            }
            return View(cvm);
        }


        public IActionResult Delete()
        {
            return PartialView("Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var _user = await _userManager.GetUserAsync(User);

            await _certificateService.DeleteCertificateAsync(id, _user.Id);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Share()
        {
            var _user = await _userManager.GetUserAsync(User);

            var callbackUrl = Url.Action(
                        "Index",
                        "Public",
                        new { uniqueUrl = _user.UniqueUrl },
                        protocol: Request.Scheme);

            return Json(callbackUrl);
        }
    }
}
