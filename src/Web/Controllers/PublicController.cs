using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Web.Interfaces;

namespace Web.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class PublicController : Controller
    {
        private readonly IPublicViewModelService _service;
        private readonly IUserRepository _repository;

        public PublicController(IPublicViewModelService service, IUserRepository repository)
        {
            _service = service;
            _repository = repository;
        }


        [Route("{uniqueUrl}")]
        public async Task<IActionResult> Index(string uniqueUrl,
                                               CancellationToken cancellationToken,
                                               string year = null,
                                               string find = null,
                                               Stage? stage = null,
                                               int page = 1)
        {
            var _user = await _repository.GetAsync(e => e.UniqueUrl == uniqueUrl, cancellationToken);

            if (_user is null)
            {
                return NotFound();
            }

            return View(_service.GetPublicViewModel(page, year, find, stage, _user));
        }


        [Route("[action]/{uniqueUrl}/{id?}")]
        public async Task<IActionResult> Details(string uniqueUrl, int id, CancellationToken cancellationToken, int page = 1)
        {
            var _user = await _repository.GetAsync(i => i.UniqueUrl == uniqueUrl, cancellationToken);

            if (_user is null)
            {
                return NotFound();
            }

            var certificate = await _service.GetCertificateByIdIncludeLinksAsync(page, id, _user.Id, _user.UniqueUrl);

            if (certificate is null)
            {
                return NotFound();
            }

            return View(certificate);
        }
    }
}
