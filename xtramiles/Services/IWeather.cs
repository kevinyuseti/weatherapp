namespace Services;

public interface IWeather
{
    string GetWeather(string lat, string lon);
}