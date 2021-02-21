using ApplicationCore.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminViewModelService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminViewModelService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _adminService.GetIndexAdminViewModelListAsync());
        }

        public async Task<IActionResult> Edit(string login)
        {
            var user = await _adminService.GetUserAsync(login);

            if(user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminViewModel avm)
        {
            await _adminService.EditUserRoleAsync(avm.Login, avm.Role);

            _logger.LogInformation($"Changed {avm.Login} role to {avm.Role}");

            return RedirectToAction(nameof(Index));
        }
    }
}
