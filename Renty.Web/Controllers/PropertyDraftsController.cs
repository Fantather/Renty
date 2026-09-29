using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Queries.Property;
using Renty.Web.Models.PropertyDrafts;
using Renty.Web.Models.Shared;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    [Authorize]
    [Route("my-properties")]
    public class PropertyDraftsController : Controller
    {
        private readonly IMediator _mediator;
        public PropertyDraftsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            // TODO: заменить на MediatR-запрос, когда будет готов бекенд.
            // Обратите внимание: для карточек на этой странице обязательно передавать ActionUrl —
            // черновики ведут на PropertyCreate/PropertyAddress/{id}, опубликованные на PropertyEdit/Title/{id}.
            var result = await _mediator.Send(new GetUserPropertiesQuery(CurrentUserId()));

            // TODO: сделать страницу с отображением полей с UserPropertyCardViewModel
            var vm = new PropertyDraftsViewModel
            {
                //Drafts = new List<UserPropertyCardViewModel>
                //{
                //    new()
                //    {
                //        Id = Guid.Parse("11111111-0000-0000-0000-000000000001"),
                //        City = "Москва",
                //        Country = "Россия",
                //        CategoryName = "Квартира",
                //        ActionUrl = Url.Action("PropertyAddress", "PropertyCreate", new { id = Guid.Parse("11111111-0000-0000-0000-000000000001") }),
                //    },
                //    new()
                //    {
                //        Id = Guid.Parse("11111111-0000-0000-0000-000000000002"),
                //        City = "Санкт-Петербург",
                //        Country = "Россия",
                //        ActionUrl = Url.Action("PropertyAddress", "PropertyCreate", new { id = Guid.Parse("11111111-0000-0000-0000-000000000002") }),
                //    },
                //},
                //Published = new List<UserPropertyCardViewModel>
                //{
                //    new()
                //    {
                //        Id = Guid.Parse("22222222-0000-0000-0000-000000000001"),
                //        City = "Сочи",
                //        Country = "Россия",
                //        CategoryName = "Студия",
                //        PricePerNight = 3500,
                //        Rating = 4.8m,
                //        ActionUrl = Url.Action("Title", "PropertyEdit", new { id = Guid.Parse("22222222-0000-0000-0000-000000000001") }),
                //    },
                //},
                Drafts = result.Data!.Drafts.Select(p => new UserPropertyCardViewModel
                {
                    Id = p.Id,
                    City = p.City,
                    Country = p.Country,
                    CreatedAt = p.CreatedAt,
                    ActionUrl = Url.Action("PropertyAddress", "PropertyCreate", new { id = p.Id})
                }).ToList(),
                Published = result.Data!.Published.Select(p => new UserPropertyCardViewModel
                {
                    Id = p.Id,
                    CategoryName = p.CategoryName,
                    PricePerNight = p.PricePerNight,
                    Rating = p.Rating,
                    City = p.City,
                    Country = p.Country,
                    CreatedAt = p.CreatedAt,
                    ActionUrl = Url.Action("Title", "PropertyEdit", new { id = p.Id })
                }).ToList()
            };

            return View(vm);
        }
        private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
