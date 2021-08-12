using System;

namespace Web.Extensions
{
    public static class CacheHelper
    {
        public static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(12);

        public static string GenerateCacheKey(this string objectName, params string[] args) 
            => objectName + "_" + string.Join('_', args);
    }
}
