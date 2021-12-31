using ApplicationCore.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Interfaces;

namespace Web.Controllers;

// TODO: move to other project!
[Authorize(Roles = Roles.Admin)]
public class AdminController : Controller
{
    private readonly IAdminViewModelService _adminService;

    public AdminController(IAdminViewModelService adminService)
    {
        _adminService = adminService;
    }


    public async Task<IActionResult> Index()
    {
        return View(await _adminService.GetIndexAdminViewModelListAsync());
    }
}