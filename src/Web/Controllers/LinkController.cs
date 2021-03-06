﻿using ApplicationCore.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public async Task<IActionResult> Index(int id)
        {
            var _user = await _userManager.GetUserAsync(User);

            return View(await _linkService.GetLinkListViewModelAsync(id, _user.Id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LinkListViewModel lvm)
        {
            var _user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                await _linkService.CreateLinkAsync(lvm.CertificateId, lvm.Link, _user.Id);

                _logger.LogInformation($"Link for the certificate {lvm.CertificateId} is created by User {_user.Id}");
            }

            return RedirectToAction("Index", new { id = lvm.CertificateId });
        }

        public IActionResult Delete()
        {
            return PartialView("Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var _user = await _userManager.GetUserAsync(User);

            int certificateId = await _linkService.DeleteLinkAsync(id, _user.Id);

            _logger.LogInformation($"Link for the certificate {certificateId} is deleted by User {_user.Id}");

            return RedirectToAction("Index", new { id = certificateId });
        }
    }
}
