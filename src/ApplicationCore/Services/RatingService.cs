using ApplicationCore.Entities.Identity;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUserRepository _userRepository;

        public RatingService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetTopTen()
        {
            var users = _userRepository.ListIncludeCertificates(i => i.OpenData && i.EmailConfirmed).ToList();
            var result = new List<User>();
            int temp = 0;

            for (int i = 0; i < users.Count(); i++)
            {
                users[i].Certificates.ForEach(i =>
                {
                    temp += i.Rating;
                });
                users[i].Rating = temp;

                result.Add(users[i]);

                temp = 0;
            }

            return result.OrderByDescending(i => i.Rating).Take(10);
        }
    }
}
