using ApplicationCore.Entities;
using ApplicationCore.Entities.Identity;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces;

public interface IPublicViewModelService
{
    Task<PublicViewModel> GetPublicViewModel(int page, string year, string find, Stage? stage,
                                       ApplicationUser user);

    Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int page, string id, string userId, string url);
}
