using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Interfaces
{
    public interface ICachedPublicViewModelService
    {
        Task<Certificate> GetItemAsync(int id, string userId);
        List<Certificate> GetList(string userId);
        Task SetItemAsync(int id, string userId);
        void SetList(string userId);
    }
}
