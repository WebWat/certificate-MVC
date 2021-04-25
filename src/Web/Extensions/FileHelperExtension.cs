using Microsoft.AspNetCore.Http;

namespace Web.Extensions
{
    public static class FileHelperExtension
    {
        public static bool CheckFileExtension(this IFormFile file, string expansion) => file.ContentType != expansion;

        public static bool CheckFileSize(this IFormFile file, long min, long max) => file.Length > max || file.Length < min;
    }
}
