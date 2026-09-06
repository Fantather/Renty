using Microsoft.AspNetCore.Mvc;
using Renty.Infrastructure.Services;

namespace Renty.Web.Controllers
{
    [Route("[controller]")]
    public class CityController : Controller
    {
        private readonly CountryStateCityAPI _CSCAPI;
        public CityController(CountryStateCityAPI CSCAPI)
        {
            _CSCAPI = CSCAPI;
        }
        public async Task<IActionResult> Index()
        {
            var citiest = await _CSCAPI.GetAllCity();

            return View(citiest);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCity(string searchTerm)
        {
            var cities = await _CSCAPI.GetCitiesBySearchTerm(searchTerm);

            return View(nameof(Index),cities);
        }

        [HttpGet("country_iso2={country_iso2:alpha}")]
        public async Task<IActionResult> GetState(string country_iso2)
        {
            return View();
        }
    }
}
