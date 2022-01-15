using ApplicationCore.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PieroDeTomi.EntityFrameworkCore.Identity.Cosmos.Contracts;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IStringLocalizer<HomeController> _localizer;
    IRepository _roleStore;

    public HomeController(IStringLocalizer<HomeController> localizer, IRepository roleStrore)
    {
        _localizer = localizer;
        _roleStore = roleStrore;
    }


    public async Task<IActionResult> Index()
    {
        return View();
    }


    public IActionResult Term()
    {
        return View();
    }


    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(CookieNamesConstants.Culture,
                                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

        return LocalRedirect(returnUrl);
    }


    [Route("/HttpError")]
    public IActionResult HttpErrorPage(string code)
    {
        switch (code)
        {
            case "404":
                var model = new HttpErrorViewModel { Title = _localizer["Title"], Error = "404", Description = _localizer["Description"], Back = _localizer["Back"] };
                return View(model);
            default:
                return NoContent();
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
