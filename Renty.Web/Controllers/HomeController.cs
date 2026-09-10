using MediatR;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Queries;
using Renty.Web.Models;
using Renty.Web.Models.Home;
using Renty.Web.Models.Shared;
using System.Diagnostics;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(PropertyFilterViewModel filter)
        {
            var categoriesResult = await _mediator.Send(new GetCategoriesQuery());
            var categoriesVm = new List<CategoryViewModel>();

            if (categoriesResult.IsSuccess && categoriesResult.Data != null)
            {
                categoriesVm = categoriesResult.Data.Categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Slug = c.Slug,
                    Name = c.Name,

                    IconName = string.IsNullOrEmpty(c.ImageUrl) ? "star" : c.ImageUrl
                }).ToList();
            }

            Guid? currentUserId = null;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(userIdString, out Guid parsedId))
                {
                    currentUserId = parsedId;
                }
            }
            var guestCount = (filter.AdultCount ?? 0) + (filter.ChildCount ?? 0) + (filter.InfantCount ?? 0) + (filter.PetCount ?? 0);

            var propertiesQuery = new GetPropertiesQuery
            {
                CategorySlug = filter.CategorySlug,
                CheckInDate = filter.CheckInDate?.ToDateTime(TimeOnly.MinValue),
                CheckOutDate = filter.CheckOutDate?.ToDateTime(TimeOnly.MinValue),
                GuestCount = guestCount > 0 ? guestCount : null,
                Page = 1,
                PageSize = 20,
                UserId = currentUserId
            };

            var propertiesResult = await _mediator.Send(propertiesQuery);
            var propertiesVm = new List<PropertyCardViewModel>();

            if (propertiesResult.IsSuccess && propertiesResult.Data != null)
            {
                propertiesVm = propertiesResult.Data.Properties.Select(p => new PropertyCardViewModel
                {
                    Slug = p.Slug,
                    ImageUrls = new List<string> { p.CoverImage },
                    IsFavorite = p.IsFavorite,
                    City = p.CityName,
                    Country = p.CountryName,
                    Rating = p.AverageRating,
                    CategoryName = p.CategoryName,
                    DurationLabel = p.Duration,
                    PricePerNight = p.PricePerNight
                }).ToList();
            }

            //модель в html
            var vm = new HomeIndexViewModel
            {
                Properties = propertiesVm,
                CategoryStrip = new CategoryStripViewModel
                {
                    Categories = categoriesVm,
                    SelectedSlug = filter.CategorySlug,
                    Filter = filter
                },
                Filter = filter,
            };

            return View(vm);
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
