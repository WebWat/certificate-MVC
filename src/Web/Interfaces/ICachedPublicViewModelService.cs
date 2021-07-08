using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Interfaces
{
    public interface ICachedPublicViewModelService
    {
        List<Certificate> GetList(string userId);
        void SetList(string userId);

        Task<Certificate> GetItemAsync(int id, string userId);
        Task SetItemAsync(int id, string userId);
    }
}
