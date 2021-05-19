using ApplicationCore.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Web.Interfaces;

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
    }
}
