using ApplicationCore.Entities;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IPublicViewModelService
    {
        PublicViewModel GetPublicViewModel(int page, string year, string find, Stage? stage,
                                           string userId, string name, string middleName, 
                                           string surname, string code, byte[] photo);

        Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int page, int id, string userId, string url);
    }
}
