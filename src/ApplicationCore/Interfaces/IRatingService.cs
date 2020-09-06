using ApplicationCore.Entities.Identity;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Service for issuing top 10 users by rating
    /// </summary>
    public interface IRatingService
    {
        IEnumerable<User> GetTopTen();
    }
}
