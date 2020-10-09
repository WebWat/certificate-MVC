using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IPublicViewModelService
    {
        PublicViewModel GetPublicViewModel(string year, string find, string userId, string name, string middleName, string surname, string country, string code, byte[] photo);
        Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int id, string userId, string url);
    }
}
