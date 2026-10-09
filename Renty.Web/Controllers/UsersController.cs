using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Renty.Application.Commands;
using Renty.Application.Commands.EditCommands;
using Renty.Application.DTOs.GetUser;
using Renty.Application.Queries;
using Renty.Web.Models.InputModels.Users;
using Renty.Web.Models.Shared;
using Renty.Web.Models.Users;
using System.Security.Claims;

namespace Renty.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Profile(Guid? id, int page = 1)
        {
            Guid? currentUserId = null;
            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(currentUserIdClaim, out var parsed))
                currentUserId = parsed;

            // Определяем, чей профиль запрашивается
            Guid targetUserId = id ?? currentUserId ?? Guid.Empty;

            if (targetUserId == Guid.Empty)
                return BadRequest();

            // айли просмотренного профиля
            var query = new GetUserProfileQuery(targetUserId, currentUserId, page, 20);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound();

            var vm = _mapper.Map<UserProfileViewModel>(result.Data);

            return View(vm);
        }

        //FOROLGA++
        // Контроллер для подгрузки комментариев в Модалку со страницы пользователя
        [HttpGet("users/{id:guid}/reviews")]
        public async Task<IActionResult> Reviews(Guid id, [FromQuery] int page = 1)
        {
            if (id == Guid.Empty) return BadRequest();

            var query = new GetHostReviewsQuery(id, page, 10);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            var formattedReviews = result.Data.Reviews.Select(r => new
            {
                id = r.Id,
                authorName = r.Author.FullName,
                authorAvatarUrl = r.Author.AvatarUrl,
                rating = r.Rating,
                text = r.Content,
                createdAt = r.CreatedAt.ToString("dd MMMM yyyy"),
                propertySlug = r.PropertySlug,
                propertyName = r.PropertyName
            });

            return Json(new
            {
                reviews = formattedReviews,
                currentPage = result.Data.Page,
                totalPages = result.Data.TotalPages
            });
        }



        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(currentUserIdClaim, out var userId))
                return Challenge();

            // айди текущего пользователя
            var query = new GetEditUserProfileQuery(userId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return Challenge();

            var input = _mapper.Map<EditUserProfileInputModel>(result.Data!.Input);
            var availableLanguages = _mapper.Map<List<Renty.Web.Models.Shared.LanguageOptionViewModel>>(result.Data!.AvailableLanguages);

            var vm = new EditUserProfileViewModel
            {
                Input = input,
                AvailableLanguages = availableLanguages
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = "Input")] EditUserProfileInputModel input)
        {
            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(currentUserIdClaim, out var userId))
                return Challenge();
            //да, леша, это костыль
            var factKeys = ModelState.Keys.Where(k => k.Contains("Facts") && k.Contains("Value")).ToList();
            foreach (var key in factKeys)
            {
                ModelState.Remove(key);
            }

            if (!ModelState.IsValid)
            {
                // айди текущего пользователя для перезагрузки формы
                var query = new GetEditUserProfileQuery(userId);
                var result = await _mediator.Send(query);

                if (result.IsSuccess)
                {
                    var availableLanguages = _mapper.Map<List<LanguageOptionViewModel>>(result.Data!.AvailableLanguages);
                    var vm = new EditUserProfileViewModel
                    {
                        Input = input,
                        AvailableLanguages = availableLanguages
                    };
                    return View(vm);
                }

                return View(new EditUserProfileViewModel { Input = input });
            }

            var inputDto = _mapper.Map<EditUserProfileInputDto>(input);

            // айди текущего пользователя в команду обновления
            var command = new UpdateUserProfileCommand(userId, inputDto);
            var updateResult = await _mediator.Send(command);

            if (!updateResult.IsSuccess)
            {
          
                var errorMsg = "Failed to update profile";
                ModelState.AddModelError(string.Empty, errorMsg);

                // языки с передачей айди текущего пользователя для перезагрузки формы
                var query = new GetEditUserProfileQuery(userId);
                var queryResult = await _mediator.Send(query);

                var availableLanguages = queryResult.IsSuccess
                    ? _mapper.Map<List<LanguageOptionViewModel>>(queryResult.Data!.AvailableLanguages)
                    : new();

                var vm = new EditUserProfileViewModel
                {
                    Input = input,
                    AvailableLanguages = availableLanguages
                };
                return View(vm);
            }

            return RedirectToAction("Profile", new { id = userId });
        }

        [HttpPost("users-avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarInputModel model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("Файл не выбран");

            using var stream = model.File.OpenReadStream();

            var command = new UploadAvatarCommand(stream, model.File.FileName);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(500, string.Join("; ", result.Errors));

            return Json(new { avatarUrl = result.Data });
        }
    }
}
