using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface ICertificateViewModelService
    {
        IndexViewModel GetIndexViewModel(string userId, string year, string find);

        Task UpdateCertificateAsync(CertificateViewModel cvm, string userId);
        Task CreateCertificateAsync(CertificateViewModel cvm, string userId);
        Task DeleteCertificateAsync(int id, string userId);

        Task<CertificateViewModel> GetCertificateByIdIncludeLinksAsync(int id, string userId);
        Task<CertificateViewModel> GetCertificateByIdAsync(int id, string userId);
    }
}
