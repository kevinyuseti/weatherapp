using CountriesInfo;

namespace Services;

public static class RegionInfo
{
    private static Dictionary<string, Models.City> _allCityInfo = new Dictionary<string, Models.City>();
    private static Dictionary<string, bool> _fetchedCountries = new Dictionary<string, bool>();
    public static bool IsCountryFetched(string code)
    {
        return _fetchedCountries.ContainsKey(code);
    }
    public static void InsertCityInfo(List<Models.City> input)
    {
        foreach (var v in input)
            {
                //lazy load, assume user always hit country first. if not, break on get weather
                _allCityInfo.Add(v.Name, new Models.City
                {
                    Name = v.Name,
                    Lon = v.Lon,
                    Lat = v.Lat
                });
            }
    }
    public static (string, string) GetCityLocation(string cityName)
    {
        if (!_allCityInfo.ContainsKey(cityName)){
            return ("", "");
        }
        return (_allCityInfo[cityName].Lat, _allCityInfo[cityName].Lon);
    }
}