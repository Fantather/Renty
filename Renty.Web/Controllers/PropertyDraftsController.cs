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
            // Для карточек на этой странице обязательно передавать ActionUrl —
            // и черновики, и опубликованные ведут на PropertyEdit/Title/{id}.
            var result = await _mediator.Send(new GetUserPropertiesQuery(CurrentUserId()));

            var vm = new PropertyDraftsViewModel
            {
                Drafts = result.Data!.Drafts.Select(p => new PropertyCardViewModel
                {
                    Id = p.Id,
                    City = p.City,
                    Country = p.Country,
                    ActionUrl = Url.Action("Title", "PropertyEdit", new { id = p.Id }),
                    ShowFavorite = false
                }).ToList(),
                Published = result.Data!.Published.Select(p => new PropertyCardViewModel
                {
                    Id = p.Id,
                    CategoryName = p.CategoryName,
                    PricePerNight = p.PricePerNight,
                    Rating = p.Rating,
                    City = p.City,
                    Country = p.Country,
                    ActionUrl = Url.Action("Title", "PropertyEdit", new { id = p.Id }),
                    ShowFavorite = false
                }).ToList()
            };

            return View(vm);
        }
        private Guid CurrentUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
