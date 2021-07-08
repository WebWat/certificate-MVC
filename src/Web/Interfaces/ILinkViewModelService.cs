using System.Threading;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface ILinkViewModelService
    {
        Task CreateLinkAsync(int certificateId, LinkViewModel cvm, string userId, 
                             CancellationToken cancellationToken = default);

        Task<int> DeleteLinkAsync(int id, string userId, CancellationToken cancellationToken = default);

        Task<LinkListViewModel> GetLinkListViewModelAsync(int certificateId, string userId, 
                                                          CancellationToken cancellationToken = default);
    }
}
