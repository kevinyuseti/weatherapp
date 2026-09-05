using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Services;
using Xunit;

namespace xtramiles.Tests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly string _responseJson;

    public MockHttpMessageHandler(string responseJson)
    {
        _responseJson = responseJson;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_responseJson)
        };
        return Task.FromResult(response);
    }
}

public class MockConfiguration : IConfiguration
{
    private readonly Dictionary<string, string?> _values;

    public MockConfiguration(Dictionary<string, string?> values)
    {
        _values = values;
    }

    public string? this[string key]
    {
        get => _values.TryGetValue(key, out var value) ? value : null;
        set => _values[key] = value;
    }

    public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();
    public IChangeToken GetReloadToken() => throw new NotImplementedException();
    public IConfigurationSection GetSection(string key) => throw new NotImplementedException();
}

public class OpenWeatherTests
{
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
                ""temp"": 18.06,
                ""feels_like"": 18.17,
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
    public void GetWeather_ParsesResponseIntoWeatherResponse()
    {
        var handler = new MockHttpMessageHandler(SampleJson);
        var client = new HttpClient(handler);
        var config = new MockConfiguration(new Dictionary<string, string?>
        {
            ["ApiKey:OpenWeather"] = "test-key"
        });
        var weather = new OpenWeather(client, config);

        var result = weather.GetWeather("52.2297", "21.0122");

        Assert.Equal(52.2297, result.Lat);
        Assert.Equal(21.0122, result.Lon);
        Assert.Equal("Europe/Warsaw", result.Timezone);
        Assert.Equal(7200, result.TimezoneOffset);

        var data = Assert.Single(result.Data);
        Assert.Equal(18.06, data.Temp);
        Assert.Equal(18.17, data.FeelsLike);
        Assert.Equal(15.68, data.DewPoint);
        Assert.Equal(6.69, data.WindSpeed);
        Assert.Equal(250, data.WindDeg);

        var condition = Assert.Single(data.Weather);
        Assert.Equal("Clouds", condition.Main);
        Assert.Equal("overcast clouds", condition.Description);
    }
}
