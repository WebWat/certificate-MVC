using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index(string uniqueUrl, string year, string find, int page = 1)
        {
            var _user = await _repository.GetAsync(i => EF.Functions.Collate(i.UniqueUrl, "SQL_Latin1_General_CP1_CS_AS") == uniqueUrl);

            if (_user == null)
            {
                return NotFound();
            }

            return View(_service.GetPublicViewModel(page, year, find, _user.Id, _user.Name, _user.MiddleName, _user.Surname, _user.UniqueUrl, _user.Photo));
        }

        [Route("[action]/{uniqueUrl}/{id?}")]
        public async Task<IActionResult> Details(string uniqueUrl, int id, int page = 1)
        {
            var _user = await _repository.GetAsync(i => i.UniqueUrl == uniqueUrl);

            if (_user == null)
            {
                return NotFound();
            }

            var certificate = await _service.GetCertificateByIdIncludeLinksAsync(page, id, _user.Id, _user.UniqueUrl);

            if (certificate == null)
            {
                return NotFound();
            }

            return View(certificate);
        }
    }
}
