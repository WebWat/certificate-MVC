using Web.Helpers;
using Xunit;

namespace UnitTests.Web.Helpers
{
    public class GenerateCacheKey
    {
        [Theory]
        [InlineData("key_arg1_arg2_arg3", "key", "arg1", "arg2", "arg3")]
        [InlineData("key_", "key")]
        public void GenerateKey(string expected, string objectName, params string[] args)
        {
            // Assert
            Assert.Equal(expected, CacheHelper.GenerateCacheKey(objectName, args));
        }
    }
}
