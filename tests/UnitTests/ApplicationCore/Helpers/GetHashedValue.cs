using ApplicationCore.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.ApplicationCore.Helpers;

public class GetHashedValue
{
    private readonly ITestOutputHelper _output;

    public GetHashedValue(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public void ReturnRandomValue()
    {
        // Arrange & Act
        var result = Sha512Helper.GetRandomValue();

        _output.WriteLine("Generated hash: " + result);

        // Assert
        Assert.Equal(128, result.Length);
    }
}
