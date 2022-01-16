using System.Threading;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces;

public interface ILinkViewModelService
{
    Task<bool> CreateLinkAsync(LinkViewModel lvm,
                               string userId,
                               CancellationToken cancellationToken = default);

    Task<LinkListViewModel> GetLinkListViewModelAsync(string certificateId,
                                                      string userId,
                                                      CancellationToken cancellationToken = default);

    Task<bool> DeleteLinkAsync(string id,
                               string certificateId,
                               string userId,
                               CancellationToken cancellationToken = default);
}
