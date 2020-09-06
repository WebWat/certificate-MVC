using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class RatingViewModelService : IRatingViewModelService
    {
        private readonly IRatingService _ratingService;

        public RatingViewModelService(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        public IEnumerable<UserViewModel> GetTopTenUsers()
        {
            return _ratingService.GetTopTen().Select(i =>
            {
                var user = new UserViewModel
                {
                    Name = i.Name,
                    Country = i.Country,
                    Rating = i.Rating
                };

                return user;
            });
        }
    }
}
