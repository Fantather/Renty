using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models.InputModels.Properties;

namespace Renty.Web.Controllers
{
    [Route("create-property")]
    public class PropertyCreateController : Controller
    {
        [HttpGet("address")]
        public IActionResult Address()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("category")]
        public IActionResult Category()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("basics")]
        public IActionResult Basics()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("amenities")]
        public IActionResult Amenities()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("photos")]
        public IActionResult Photos()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("title")]
        public IActionResult Title()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("highlights")]
        public IActionResult Highlights()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("description")]
        public IActionResult Description()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("booking")]
        public IActionResult Booking()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("pricing")]
        public IActionResult Pricing()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("discounts")]
        public IActionResult Discounts()
        {
            return View(new PropertyInputModel());
        }

        [HttpGet("review")]
        public IActionResult Review()
        {
            return View(new PropertyInputModel());
        }

        // Заглушка: показывает ожидаемую форму ответа для поиска адреса.
        [HttpGet("search-address")]
        public IActionResult SearchAddress(string searchTerm)
        {
            return Json(new[]
            {
                new { title = "Пример, Одесса", address = "ул. Примерная, 1", street = "ул. Примерная", district = "Приморский", cityId = "Одесса", countryId = "Украина" },
            });
        }
    }
}
