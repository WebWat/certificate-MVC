using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;

namespace Web.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingViewModelService _ratingService;

        public RatingController(IRatingViewModelService ratingService)
        {
            _ratingService = ratingService;
        }

        public IActionResult Index()
        {
            return View(_ratingService.GetTopTenUsers());
        }
    }
}
