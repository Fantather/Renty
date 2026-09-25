using MediatR;
using Renty.Application.Common;
using Renty.Application.Queries.Autocomplete;
using Renty.Domain.Interfaces;
using Renty.Domain.ServiceModels.Places;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.AutocompleteHandlers
{
    public class AutocompleteCityHandler : IRequestHandler<AutocompleteCityQuery, OperationResult<List<CitySuggestionDto>>>
    {
        private readonly IAddressAutocompleteService _autocompleteService;
        public AutocompleteCityHandler(IAddressAutocompleteService autocompleteService)
        {
            _autocompleteService = autocompleteService;
        }
        public async Task<OperationResult<List<CitySuggestionDto>>> Handle(AutocompleteCityQuery request, CancellationToken cancellationToken)
        {
            int minLength = 3;

            if (string.IsNullOrEmpty(request.Input) || request.Input.Length < minLength)
                return OperationResult<List<CitySuggestionDto>>.Success(new());

            var result = await _autocompleteService.SearchCities(request.Input, request.SessionToken, ct: cancellationToken);

            return OperationResult<List<CitySuggestionDto>>.Success(result);
        }
    }
}
