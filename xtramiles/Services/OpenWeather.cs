using System.Net.Http;
using Models;
using Newtonsoft.Json;

namespace Services;

public class OpenWeather : IWeather
{
    private readonly HttpClient _client;
    private string _apiKey;
    public OpenWeather(HttpClient client, IConfiguration config)
    {
        _client = client;
        _apiKey = config["ApiKey:OpenWeather"].ToString();
    }
    public WeatherResponse GetWeather(string lat, string lon)
    {
        var response = _client.GetAsync($"https://api.openweathermap.org/data/4.0/onecall/current?lat={lat}&lon={lon}&units=imperial&lang=en&appid={_apiKey}").Result;
        if (!response.IsSuccessStatusCode)
        {
            return new WeatherResponse();
        }
        var resp = response.Content.ReadAsStringAsync().Result;
        WeatherResponse data = JsonConvert.DeserializeObject<Models.WeatherResponse>(resp);
        return data;
    }
}