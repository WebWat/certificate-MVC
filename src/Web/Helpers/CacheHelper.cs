namespace Web.Helpers
{
    public static class CacheHelper
    {
        public static string GenerateCacheKey(string objectName, params string[] args) => objectName + "_" + string.Join('_', args);
    }
}
