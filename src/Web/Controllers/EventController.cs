using ApplicationCore.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Interfaces;

namespace Web.Controllers
{
    [Authorize(Roles = Roles.User)]
    public class EventController : Controller
    {
        private readonly IEventViewModelService _eventService;

        public EventController(IEventViewModelService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _eventService.GetEventListAsync(page));
        }
    }
}
