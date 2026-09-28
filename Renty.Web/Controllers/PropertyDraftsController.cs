using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models.PropertyDrafts;
using Renty.Web.Models.Shared;

namespace Renty.Web.Controllers
{
    [Authorize]
    [Route("my-properties")]
    public class PropertyDraftsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // TODO: заменить на MediatR-запрос, когда будет готов бекенд.
            // Обратите внимание: для карточек на этой странице обязательно передавать ActionUrl —
            // черновики ведут на PropertyCreate/PropertyAddress/{id}, опубликованные на PropertyEdit/Title/{id}.
            var vm = new PropertyDraftsViewModel
            {
                Drafts = new List<PropertyCardViewModel>
                {
                    new()
                    {
                        Id = Guid.Parse("11111111-0000-0000-0000-000000000001"),
                        City = "Москва",
                        Country = "Россия",
                        CategoryName = "Квартира",
                        ActionUrl = Url.Action("PropertyAddress", "PropertyCreate", new { id = Guid.Parse("11111111-0000-0000-0000-000000000001") }),
                    },
                    new()
                    {
                        Id = Guid.Parse("11111111-0000-0000-0000-000000000002"),
                        City = "Санкт-Петербург",
                        Country = "Россия",
                        ActionUrl = Url.Action("PropertyAddress", "PropertyCreate", new { id = Guid.Parse("11111111-0000-0000-0000-000000000002") }),
                    },
                },
                Published = new List<PropertyCardViewModel>
                {
                    new()
                    {
                        Id = Guid.Parse("22222222-0000-0000-0000-000000000001"),
                        City = "Сочи",
                        Country = "Россия",
                        CategoryName = "Студия",
                        PricePerNight = 3500,
                        Rating = 4.8m,
                        ActionUrl = Url.Action("Title", "PropertyEdit", new { id = Guid.Parse("22222222-0000-0000-0000-000000000001") }),
                    },
                },
            };

            return View(vm);
        }
    }
}
