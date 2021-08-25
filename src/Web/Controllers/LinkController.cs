using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class LinkController : Controller
    {
        private readonly ILinkViewModelService _linkService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LinkController> _logger;

        public LinkController(ILinkViewModelService linkService,
                              UserManager<ApplicationUser> userManager,
                              ILogger<LinkController> logger)
        {
            _linkService = linkService;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
        {
            var _user = await _userManager.GetUserAsync(User);

            var result = await _linkService.GetLinkListViewModelAsync(id, _user.Id, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return View(result);
        }


        public IActionResult Create(int id)
        {
            return View(new LinkViewModel { CertificateId = id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LinkViewModel lvm, CancellationToken cancellationToken)
        {
            var _user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var success = await _linkService.CreateLinkAsync(lvm, _user.Id, cancellationToken);

                if (!success)
                {
                    return BadRequest();
                }

                _logger.LogInformation($"Link for the certificate {lvm.CertificateId} is created by User {_user.Id}");
            }

            return RedirectToAction(nameof(Index), new { id = lvm.CertificateId });
        }


        public IActionResult Delete()
        {
            return PartialView(nameof(Delete));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int certificateId, CancellationToken cancellationToken)
        {
            var _user = await _userManager.GetUserAsync(User);

            var success = await _linkService.DeleteLinkAsync(id, certificateId, _user.Id, cancellationToken);

            if (!success)
            {
                return NotFound();
            }

            _logger.LogInformation($"Link for the certificate {certificateId} is deleted by User {_user.Id}");

            return RedirectToAction(nameof(Index), new { id = certificateId });
        }
    }
}
