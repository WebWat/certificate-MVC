using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface ILinkViewModelService
    {
        Task CreateLinkAsync(int certificateId, LinkViewModel cvm, string userId);
        Task<int> DeleteLinkAsync(int id, string userId);
        Task<LinkListViewModel> GetLinkListViewModelAsync(int certificateId, string userId);
    }
}
