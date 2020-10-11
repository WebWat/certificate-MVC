using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 600)]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 600)]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/HttpError")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 600)]
        public IActionResult HttpErrorPage(string code)
        {
            if (code == "404")
            {
                var model = new HttpErrorViewModel { Title = "Не найдено", Error = "404", Description = "Страница не найдена" };
                return View(model);
            }

            return NoContent();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
