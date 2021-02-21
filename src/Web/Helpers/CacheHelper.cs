using System;

namespace Web.Helpers
{
    public static class CacheHelper
    {
        public static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(12);

        public static string GenerateCacheKey(string objectName, params string[] args) 
            => objectName + "_" + string.Join('_', args);
    }
}
