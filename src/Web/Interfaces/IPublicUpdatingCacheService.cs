using System.Threading.Tasks;

namespace Web.Interfaces
{
    public interface IPublicUpdatingCacheService
    {
        public Task SetItemAsync(int id, string userId);
        public void SetList(string userId);
    }
}
