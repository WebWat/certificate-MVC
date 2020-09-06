using System.Collections.Generic;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IRatingViewModelService
    {
        IEnumerable<UserViewModel> GetTopTenUsers();
    }
}
