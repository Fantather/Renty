using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Queries;
using Renty.Application.Queries.Autocomplete;
using Renty.Domain.ServiceModels.Places;

namespace Renty.Web.Controllers.Api
{
    [Route("api/places")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PlacesController(IMediator mediator)
        {
            _mediator = mediator;   
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> AutoComplete([FromQuery]string input, [FromQuery]string sessionToken, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(input))
                return Ok(new List<AddressSuggestionDto>());
            var result = await _mediator.Send(new AutocompleteQuery(input, sessionToken),ct);

            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
        }
        // Авто заполнение для города
        [HttpGet("autocomplete-city")]
        public async Task<IActionResult> AutoCompleteCity([FromQuery]string input, [FromQuery]string sessionToken, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(input))
                return Ok(new List<CitySuggestionDto>());
            var result = await _mediator.Send(new AutocompleteCityQuery(input, sessionToken),ct);

            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
        }
        // Сюда идет запрос после выбора города из автозаполнения или ввода названия города вручную
        [HttpGet("resolve-city")]
        public async Task<IActionResult> ResolveCity([FromQuery] string? placeId, [FromQuery] string? cityText, CancellationToken ct)
        {
            var result = await _mediator.Send(new ResolveCityQuery(placeId, cityText), ct);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Errors);
        }
    }
}
