using ApplicationCore.Constants;
using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Interfaces;
using Web.Models;
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
        private readonly FileSettings _fileSettings;

        public CertificateController(ICertificateViewModelService certificateService, 
                                     UserManager<ApplicationUser> userManager, 
                                     IStringLocalizer<SharedResource> localizer,
                                     ILogger<CertificateController> logger,
                                     IOptions<FileSettings> options)
        {
            _certificateService = certificateService;
            _userManager = userManager;
            _localizer = localizer;
            _logger = logger;
            _fileSettings = options.Value;
        }


        public async Task<IActionResult> Index(int page = 1, string year = null, string find = null)
        {
            var _user = await _userManager.GetUserAsync(User);

            return View(_certificateService.GetIndexViewModel(page, _user.Id, year, find));
        }


        public async Task<IActionResult> Details(int id, int page = 0)
        {
            page = GetCurrentPage(page);

            var _user = await _userManager.GetUserAsync(User);

            var certificate = await _certificateService.GetCertificateByIdIncludeLinksAsync(page, id, _user.Id);

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
                    if (cvm.File.CheckFileExtension(_fileSettings.Expansion)) 
                    {
                        ModelState.AddModelError("File", _localizer["FileExtensionError"]);
                        return View();
                    }

                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(cvm.File.OpenReadStream()))
                    {

                        if (cvm.File.CheckFileSize(_fileSettings.MinSize, _fileSettings.SizeLimit))
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
                    if (cvm.File.CheckFileExtension(_fileSettings.Expansion))
                    {
                        ModelState.AddModelError("File", _localizer["FileExtensionError"]);
                        return View(cvm);
                    }

                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(cvm.File.OpenReadStream()))
                    {
                        if (cvm.File.CheckFileSize(_fileSettings.MinSize, _fileSettings.SizeLimit))
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
            int currentPage = GetCurrentPage();

            if (HttpContext.Request.Cookies["isLast"] == "true")
            {
                if (currentPage > 1)
                    currentPage -= 1;

                HttpContext.Response.Cookies.Append("isLast", "false", new() { SameSite = SameSiteMode.Lax });
            }

            var _user = await _userManager.GetUserAsync(User);

            await _certificateService.DeleteCertificateAsync(id, _user.Id);

            _logger.LogInformation($"Certificate {id} deleted by User {_user.Id}");

            return RedirectToAction(nameof(Index), new { page = currentPage });
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

        private int GetCurrentPage(int page = 0)
        {
            if (page == 0)
            {
                page = int.TryParse(HttpContext.Request.Cookies["page"], out int result) ? result : 1;
            }
            else
            {
                HttpContext.Response.Cookies.Append("page", page.ToString(), new() { SameSite = SameSiteMode.Lax });
            }

            return page;
        }
    }
}
