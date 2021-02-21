using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    /// <summary>
    /// Returns the top 10 users with the highest rating
    /// </summary>
    public class RatingService : IRatingService
    {
        private readonly IUserRepository _userRepository;

        public RatingService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<ApplicationUser> GetTopTen()
        {
            //We can easily change our rating logic
#if DEBUG
            var users = _userRepository.ListIncludeCertificates(i => i.OpenData).ToList();
#elif RELEASE
            var users = _userRepository.ListIncludeCertificates(i => i.OpenData && i.EmailConfirmed).ToList();
#endif
            return users.OrderByDescending(i => i.Rating).Take(10);
        }
    }
}
