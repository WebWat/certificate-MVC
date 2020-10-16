namespace Web.Helpers
{
    public static class CacheHelper
    {
        public static string GenerateCacheKey(string objectName, string id) => objectName + id;
    }
}
