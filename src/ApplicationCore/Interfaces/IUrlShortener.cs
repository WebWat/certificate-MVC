using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUrlShortener
    {
        Task<string> GetShortenedUrlAsync(string url);
    }
}
