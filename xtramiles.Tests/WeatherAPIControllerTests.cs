using xtramiles.Controllers;
using Xunit;

namespace xtramiles.Tests;

public class WeatherAPIControllerTests
{
    [Theory]
    [InlineData(32, 0)]
    [InlineData(212, 100)]
    [InlineData(-40, -40)]
    [InlineData(98.6, 37)]
    public void FahrenheitToCelcius(double fahrenheit, double celsius)
    {
        var result = WeatherAPIController.fahrenheitToCelcius(fahrenheit);

        Assert.Equal(celsius, result, 2);
    }
}
