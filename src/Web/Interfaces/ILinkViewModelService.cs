using System.Threading;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces;

public interface ILinkViewModelService
{
    Task<bool> CreateLinkAsync(LinkViewModel lvm,
                               string userId,
                               CancellationToken cancellationToken = default);

    Task<LinkListViewModel?> GetLinkListViewModelAsync(int certificateId,
                                                      string userId,
                                                      CancellationToken cancellationToken = default);

    Task<bool> DeleteLinkAsync(int id,
                               int certificateId,
                               string userId,
                               CancellationToken cancellationToken = default);
}
