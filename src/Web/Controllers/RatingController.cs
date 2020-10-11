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

        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 600)]
        public IActionResult Index()
        {
            return View(_ratingService.GetTopTenUsers());
        }
    }
}
