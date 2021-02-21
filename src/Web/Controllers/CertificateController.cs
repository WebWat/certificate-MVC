using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CertificateController> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly long _fileSizeLimit = 2097152;
        private readonly long _fileMinSize = 524288;
        private readonly string _expansion = "image/jpeg";

        public CertificateController(ICertificateViewModelService certificateService, 
                                     UserManager<ApplicationUser> userManager, 
                                     IStringLocalizer<SharedResource> localizer,
                                     ILogger<CertificateController> logger)
        {
            _certificateService = certificateService;
            _userManager = userManager;
            _localizer = localizer;
            _logger = logger;
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
                ModelState.AddModelError("File", _localizer["Required"]);
            }

            if (ModelState.IsValid)
            {
                if (cvm.File != null)
                {
                    if (cvm.File.CheckFileExtension(_expansion)) 
                    {
                        ModelState.AddModelError("File", _localizer["FileExtensionError"]);
                        return View();
                    }

                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(cvm.File.OpenReadStream()))
                    {

                        if (cvm.File.CheckFileSize(_fileMinSize, _fileSizeLimit))
                        {
                            ModelState.AddModelError("File", _localizer["FileSizeError"]);
                            return View();
                        }
                        imageData = binaryReader.ReadBytes((int)cvm.File.Length);
                    }

                    cvm.ImageData = imageData;
                }

                await _certificateService.CreateCertificateAsync(cvm, _user.Id);

                _logger.LogInformation($"New certificate created by User {_user.Id}");

                return RedirectToAction(nameof(Index));
            }
            return View();
        }


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
                        ModelState.AddModelError("File", _localizer["FileExtensionError"]);
                        return View(cvm);
                    }

                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(cvm.File.OpenReadStream()))
                    {
                        if (cvm.File.CheckFileSize(_fileMinSize, _fileSizeLimit))
                        {
                            ModelState.AddModelError("File", _localizer["FileSizeError"]);
                            return View(cvm);
                        }
                        imageData = binaryReader.ReadBytes((int)cvm.File.Length);
                    }

                    cvm.ImageData = imageData;
                }

                await _certificateService.UpdateCertificateAsync(cvm, _user.Id);

                _logger.LogInformation($"Certificate {cvm.Id} changed by User {_user.Id}");

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

            _logger.LogInformation($"Certificate {id} deleted by User {_user.Id}");

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
