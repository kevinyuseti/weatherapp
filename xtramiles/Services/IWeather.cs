using Models;

namespace Services;

public interface IWeather
{
    WeatherResponse GetWeather(string lat, string lon);
}