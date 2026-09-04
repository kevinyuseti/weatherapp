using System.ComponentModel;
using CountriesInfo;
using Microsoft.AspNetCore.Mvc;
using Services;
namespace xtramiles.Controllers;

[ApiController]
[Route("api")]
public class WeatherAPIController : Controller
{
    private readonly CountriesService _countryService;
    private readonly List<Tuple<string, string>> _listOfCountries;
    private IWeather weatherService;


    public WeatherAPIController(OpenWeather wService)
    {
        this._countryService = new CountriesService();
        var countries = this._countryService.GetAllCountries();
        _listOfCountries = countries.Select(c => new Tuple<string, string>(c.Codes.ISO3, c.Name.Official)).OrderBy(c1 => c1.Item2).ToList();
        this.weatherService = wService;
    }

    [Route("countries")]
    [HttpGet]
    public JsonResult Countries()
    {
        return new JsonResult(this._listOfCountries);
    }

    [Route("cities/{code}")]
    [HttpGet]
    public JsonResult Cities(string code)
    {
        List<Models.City> cities = this._countryService.GetAllCitiesInCountry(code).Select(c => new Models.City
        {
            Name = c.Name,
            Lon = c.Longitude,
            Lat = c.Latitude

        }).ToList();
        Task.Run(() =>
        {
            if (RegionInfo.IsCountryFetched(code))
            {
                return;
            }
            RegionInfo.InsertCityInfo(cities);
        });
        return new JsonResult(cities);
    }

    [Route("weather/{cityName}")]
    [HttpGet]
    public JsonResult Weather(string cityName)
    {
        var (lat, lot) = RegionInfo.GetCityLocation(cityName);
        var res = weatherService.GetWeather(lat, lot);
        return new JsonResult(res);
    }
}
