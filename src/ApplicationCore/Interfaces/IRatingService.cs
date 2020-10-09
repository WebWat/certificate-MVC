using ApplicationCore.Entities.Identity;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces
{
    public interface IRatingService
    {
        IEnumerable<User> GetTopTen();
    }
}
