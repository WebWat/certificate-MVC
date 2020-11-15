using System.Threading.Tasks;

namespace Web.Interfaces
{
    public interface IPublicUpdatingCacheService
    {
        Task SetItemAsync(int id, string userId);
        void SetList(string userId);
    }
}
