using Models;
using Services;
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

    private const string SampleJson = @"{
        ""lat"": 52.2297,
        ""lon"": 21.0122,
        ""timezone"": ""Europe/Warsaw"",
        ""timezone_offset"": 7200,
        ""data"": [
            {
                ""dt"": 1788567446,
                ""sunrise"": 1788580420,
                ""sunset"": 1788628575,
                ""temp"": 100,
                ""feels_like"": 32,
                ""pressure"": 1005,
                ""humidity"": 86,
                ""dew_point"": 15.68,
                ""uvi"": 0,
                ""clouds"": 100,
                ""visibility"": 10000,
                ""wind_speed"": 6.69,
                ""wind_deg"": 250,
                ""weather"": [
                    {
                        ""id"": 804,
                        ""main"": ""Clouds"",
                        ""description"": ""overcast clouds"",
                        ""icon"": ""04n""
                    }
                ]
            }
        ]
    }";

    [Fact]
    public void Weather_ConvertsTemperaturesToCelsius()
    {
        const string cityName = "WeatherApiControllerTests_TestCity";
        RegionInfo.InsertCityInfo(new List<City>
        {
            new City { Name = cityName, Lat = "52.2297", Lon = "21.0122" }
        });

        var httpClient = new HttpClient(new MockHttpMessageHandler(SampleJson));
        var config = new MockConfiguration(new Dictionary<string, string?>
        {
            ["ApiKey:OpenWeather"] = "test-key"
        });
        var openWeather = new OpenWeather(httpClient, config);
        var controller = new WeatherAPIController(openWeather);

        var result = controller.Weather(cityName);
        var response = Assert.IsType<WeatherResponse>(result.Value);
        var data = Assert.Single(response.Data);

        Assert.Equal(37.78, data.Temp, 2);
        Assert.Equal(0, data.FeelsLike, 2);
    }

    [Fact]
    public void Weather_WhenApiReturnsError_Throws()
    {
        const string cityName = "WeatherApiControllerTests_ErrorCity";
        RegionInfo.InsertCityInfo(new List<City>
        {
            new City { Name = cityName, Lat = "52.2297", Lon = "21.0122" }
        });

        var httpClient = new HttpClient(new MockHttpMessageHandler("", System.Net.HttpStatusCode.Unauthorized));
        var config = new MockConfiguration(new Dictionary<string, string?>
        {
            ["ApiKey:OpenWeather"] = "test-key"
        });
        var openWeather = new OpenWeather(httpClient, config);
        var controller = new WeatherAPIController(openWeather);

        Assert.Throws<ArgumentNullException>(() => controller.Weather(cityName));
    }

    [Fact]
    public void Countries_ReturnsCountryList()
    {
        var httpClient = new HttpClient(new MockHttpMessageHandler(SampleJson));
        var config = new MockConfiguration(new Dictionary<string, string?>
        {
            ["ApiKey:OpenWeather"] = "test-key"
        });
        var controller = new WeatherAPIController(new OpenWeather(httpClient, config));

        var result = controller.Countries();
        var countries = Assert.IsAssignableFrom<List<Tuple<string, string>>>(result.Value);

        Assert.Contains("IDN", countries.Select(x=>x.Item1));
    }

    [Fact]
    public void Cities_ReturnsCityList()
    {
        const string countryCode = "IDN";

        var httpClient = new HttpClient(new MockHttpMessageHandler(SampleJson));
        var config = new MockConfiguration(new Dictionary<string, string?>
        {
            ["ApiKey:OpenWeather"] = "test-key"
        });
        var controller = new WeatherAPIController(new OpenWeather(httpClient, config));

        var result = controller.Cities(countryCode);
        var cities = Assert.IsAssignableFrom<List<City>>(result.Value);

        Assert.Contains("Yogyakarta",cities.Select(x=>x.Name));
    }
}
