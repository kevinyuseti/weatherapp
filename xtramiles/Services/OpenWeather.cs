namespace Services;

public class OpenWeather : IWeather
{
    public string GetWeather(string lat, string lon)
    {
        return "foo";
    }
}