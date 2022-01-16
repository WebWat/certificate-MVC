using ApplicationCore.Constants;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using ApplicationCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Interfaces;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers;

[Authorize]
public class CertificateController : Controller
{
    private readonly ICertificateViewModelService _certificateService;
    private readonly ILogger<CertificateController> _logger;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FileSettings _fileSettings;
    private readonly IWebHostEnvironment _appEnvironment;

    public CertificateController(ICertificateViewModelService certificateService,
                                 UserManager<ApplicationUser> userManager,
                                 IStringLocalizer<SharedResource> localizer,
                                 ILogger<CertificateController> logger,
                                 IOptions<FileSettings> options,
                                 IWebHostEnvironment appEnvironment)
    {
        _certificateService = certificateService;
        _userManager = userManager;
        _localizer = localizer;
        _logger = logger;
        _fileSettings = options.Value;
        _appEnvironment = appEnvironment;
    }


    public async Task<IActionResult> Index(int page = 1,
                                           string year = null,
                                           string find = null,
                                           Stage? stage = null)
    {
        var _user = await _userManager.GetUserAsync(User);

        return View(await _certificateService.GetIndexViewModel(page, _user.Id, year, find, stage));
    }


    public async Task<IActionResult> Details(string id, CancellationToken cancellationToken, int page = 0)
    {
        page = GetCurrentPage(page);

        var _user = await _userManager.GetUserAsync(User);

        var certificate = await _certificateService.GetCertificateByIdIncludeLinksAsync(page,
                                                                                        id,
                                                                                        _user.Id,
                                                                                        cancellationToken);

        if (certificate is null)
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
    public async Task<IActionResult> Create(CertificateViewModel cvm, CancellationToken cancellationToken)
    {
        var _user = await _userManager.GetUserAsync(User);

        if (cvm.File is null)
        {
            ModelState.AddModelError("File", _localizer["Required"]);
        }

        if (ModelState.IsValid)
        {
            if (cvm.File.CheckFileExtension(_fileSettings.Expansion))
            {
                ModelState.AddModelError("File", _localizer["FileExtensionError"]);
                return View();
            }

            using (var binaryReader = new BinaryReader(cvm.File.OpenReadStream()))
            {
                if (cvm.File.CheckFileSize(_fileSettings.MinSize, _fileSettings.SizeLimit))
                {
                    ModelState.AddModelError("File", _localizer["FileSizeError"]);
                    return View();
                }

                var path = $"/private/{Sha512Helper.GetRandomValue()}.jpg";

                using (var stream = System.IO.File.Create(_appEnvironment.WebRootPath + path))
                {
                    await cvm.File.CopyToAsync(stream, cancellationToken);
                }

                cvm.Path = path;
            }

            await _certificateService.CreateCertificateAsync(cvm, _user.Id, cancellationToken);

            _logger.LogInformation($"New certificate created by User {_user.Id}");

            return RedirectToAction(nameof(Index));
        }

        return View();
    }


    public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
    {
        var _user = await _userManager.GetUserAsync(User);

        var certificate = await _certificateService.GetCertificateByIdAsync(id, _user.Id, cancellationToken);

        if (certificate is null)
        {
            return NotFound();
        }

        return View(certificate);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CertificateViewModel cvm, CancellationToken cancellationToken)
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

                using (var binaryReader = new BinaryReader(cvm.File.OpenReadStream()))
                {
                    if (cvm.File.CheckFileSize(_fileSettings.MinSize, _fileSettings.SizeLimit))
                    {
                        ModelState.AddModelError("File", _localizer["FileSizeError"]);
                        return View(cvm);
                    }

                    var path = $"/private/{Sha512Helper.GetRandomValue()}.jpg";

                    using (var stream = System.IO.File.Create(_appEnvironment.WebRootPath + path))
                    {
                        await cvm.File.CopyToAsync(stream, cancellationToken);
                    }

                    cvm.Path = path;
                }
            }

            try
            {
                await _certificateService.UpdateCertificateAsync(cvm, _user.Id, cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("An error occurred while updating the certificate: " + ex.Message);

                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation($"Certificate {cvm.Id} changed by User {_user.Id}");

            return RedirectToAction(nameof(Details), new { id = cvm.Id });
        }

        return View(cvm);
    }


    public IActionResult Delete()
    {
        return PartialView(nameof(Delete));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        int currentPage = GetCurrentPage();

        var _user = await _userManager.GetUserAsync(User);

        await _certificateService.DeleteCertificateAsync(id, _user.Id, cancellationToken);

        _logger.LogInformation($"Certificate {id} deleted by User {_user.Id}");

        return RedirectToAction(nameof(Index), new { page = currentPage });
    }


    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> Share()
    {
        var _user = await _userManager.GetUserAsync(User);

        var callbackUrl = Url.Action("Index",
                                     "Public",
                                     new { uniqueUrl = _user.UniqueUrl },
                                     protocol: Request.Scheme);

        return Json(callbackUrl);
    }


    private int GetCurrentPage(int page = 0)
    {
        if (page == 0)
        {
            // Try get page from cookies.
            page = int.TryParse(HttpContext.Request.Cookies[CookieNamesConstants.Page],
                                out int result) ? result : 1;
        }
        else
        {
            HttpContext.Response.Cookies.Append(CookieNamesConstants.Page, 
                                                page.ToString(),
                                                new() { SameSite = SameSiteMode.Lax });
        }

        return page;
    }
}