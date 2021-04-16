using ApplicationCore.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = Roles.AdminAndModerator)]
    [IgnoreAntiforgeryToken]
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
            await _moderatorService.DeleteEventAsync(id);

            _logger.LogInformation($"Event {id} deleted");

            return RedirectToAction(nameof(Index));
        }
    }
}
