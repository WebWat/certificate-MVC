using System.Collections.Generic;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IAdminViewModelService
    {
        Task<IEnumerable<AdminViewModel>> GetIndexAdminViewModelListAsync();
        Task EditUserRoleAsync(string login, string role);
        Task DeleteUserAsync(string id);
        Task<AdminViewModel> GetUserAsync(string login);
    }
}
