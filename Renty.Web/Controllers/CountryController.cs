using Microsoft.AspNetCore.Mvc;
using Renty.Infrastructure.Services.CountryStateCityAPI;

namespace Renty.Web.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountryStateCityAPI _CSCAPI;

        public CountryController(CountryStateCityAPI CSCAPI)
        {
            _CSCAPI = CSCAPI;
        }
        public async Task<IActionResult> Index()
        {

            var result = await _CSCAPI.GetAllCountryAsync();


            return View(result);
        }
    }
}
