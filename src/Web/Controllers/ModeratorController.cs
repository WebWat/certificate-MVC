using ApplicationCore.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = Roles.AdminAndModerator)]
    public class ModeratorController : Controller
    {
        private readonly IModeratorViewModelService _moderatorService;
        private readonly ILogger<LinkController> _logger;

        public ModeratorController(IModeratorViewModelService moderatorService, ILogger<LinkController> logger)
        {
            _moderatorService = moderatorService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            HttpContext.Response.Cookies.Append("page_event", page.ToString(), new() { SameSite = SameSiteMode.Lax });

            return View(await _moderatorService.GetEventListAsync(page));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel evm)
        {
            await _moderatorService.CreateEventAsync(evm);

            _logger.LogInformation("New event created");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, int page)
        {
            HttpContext.Response.Cookies.Append("page_event", page.ToString(), new() { SameSite = SameSiteMode.Lax });

            return View(await _moderatorService.GetEventByIdAsync(id, page));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventViewModel evm)
        {
            await _moderatorService.UpdateEventAsync(evm);

            _logger.LogInformation($"Event {evm.Id} changed");

            return RedirectToAction(nameof(Index), new { page = evm.Page });
        }

        public IActionResult Delete()
        {
            return PartialView("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int currentPage = int.TryParse(HttpContext.Request.Cookies["page_event"], out int result) ? result : 1;

            if (HttpContext.Request.Cookies["event_isLast"] == "true")
            {
                if (currentPage > 1)
                    currentPage -= 1;

                HttpContext.Response.Cookies.Append("event_isLast", "false", new() { SameSite = SameSiteMode.Lax });
            }

            await _moderatorService.DeleteEventAsync(id);

            _logger.LogInformation($"Event {id} deleted");

            return RedirectToAction(nameof(Index), new { page = currentPage });
        }
    }
}
