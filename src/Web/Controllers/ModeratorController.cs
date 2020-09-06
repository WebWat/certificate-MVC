using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class ModeratorController : Controller
    {
        private readonly IModeratorViewModelService _moderatorService;

        public ModeratorController(IModeratorViewModelService moderatorService)
        {
            _moderatorService = moderatorService;
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

            return RedirectToAction(nameof(Index));
        }
    }
}
