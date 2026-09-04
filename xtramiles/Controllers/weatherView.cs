using Microsoft.AspNetCore.Mvc;
namespace xtramiles.Controllers;

[Route("home")]
public class WeatherUIController : Controller
{
    
    public WeatherUIController()
    {
    }
    [Route("countries")]
    [HttpGet]
    public async Task<ViewResult> Countries()
    {
        return View("Countries");
    }
}
