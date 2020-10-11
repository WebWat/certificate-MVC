using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminViewModelService _adminService;

        public AdminController(IAdminViewModelService adminService)
        {
            _adminService = adminService;
        }

        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 900)]
        public async Task<IActionResult> Index()
        {
            return View(await _adminService.GetIndexAdminViewModelListAsync());
        }

        public async Task<IActionResult> Edit(string login)
        {
            return View(await _adminService.GetUserAsync(login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminViewModel avm)
        {
            await _adminService.EditUserRoleAsync(avm.Login, avm.Role);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete()
        {
            return PartialView("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            await _adminService.DeleteUserAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
