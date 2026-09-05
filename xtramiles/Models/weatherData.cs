using Newtonsoft.Json;

namespace Models;

public class WeatherResponse
{
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Timezone { get; set; }

    [JsonProperty("timezone_offset")]
    public int TimezoneOffset { get; set; }

    public List<WeatherData> Data { get; set; }
}

public class WeatherData
{
    public long Dt { get; set; }
    public long Sunrise { get; set; }
    public long Sunset { get; set; }
    public double Temp { get; set; }

    [JsonProperty("feels_like")]
    public double FeelsLike { get; set; }

    public int Pressure { get; set; }
    public int Humidity { get; set; }

    [JsonProperty("dew_point")]
    public double DewPoint { get; set; }

    public double Uvi { get; set; }
    public int Clouds { get; set; }
    public int Visibility { get; set; }

    [JsonProperty("wind_speed")]
    public double WindSpeed { get; set; }

    [JsonProperty("wind_deg")]
    public int WindDeg { get; set; }

    public List<WeatherCondition> Weather { get; set; }
}

public class WeatherCondition
{
    public int Id { get; set; }
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}
