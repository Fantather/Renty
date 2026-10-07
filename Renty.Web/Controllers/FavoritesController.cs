using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Commands;
using Renty.Web.Models.Shared;
using System.Security.Claims;
using Renty.Application.Queries;

namespace Renty.Web.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FavoritesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Возвращает страницу со списком избранного пользователя
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(new GetUserFavoritesQuery(userId));

            if (!result.IsSuccess)
            {
                return View(new List<PropertyCardViewModel>());
            }

            var viewModel = _mapper.Map<List<PropertyCardViewModel>>(result.Data);

            return View(viewModel);
        }

        /// <summary>
        /// AJAX-метод для переключения статуса избранного (добавить/удалить)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite([FromBody] string slug)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Unauthorized(new { success = false, message = "User not logged in" });
            }

            var userId = Guid.Parse(userIdStr);

            var result = await _mediator.Send(new ToggleFavoriteCommand(userId, slug));

            if (!result.IsSuccess)
            {
                return BadRequest(new { success = false, message = string.Join(", ", result.Errors) });
            }

            return Ok(new { success = true, isFavorite = result.Data });
        }
    }
}
