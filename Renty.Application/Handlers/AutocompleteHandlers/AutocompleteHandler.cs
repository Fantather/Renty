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
    public class AutocompleteHandler : IRequestHandler<AutocompleteQuery, OperationResult<List<AddressSuggestionDto>>>
    {
        private readonly IAddressAutocompleteService _autocompleteService;
        public AutocompleteHandler(IAddressAutocompleteService autocompleteService)
        {
            _autocompleteService = autocompleteService;
        }
        public async Task<OperationResult<List<AddressSuggestionDto>>> Handle(AutocompleteQuery request, CancellationToken cancellationToken)
        {
            int minLength = 3;

            if (string.IsNullOrEmpty(request.Input) || request.Input.Length < minLength)
                return OperationResult<List<AddressSuggestionDto>>.Success(new ());

            var result = await _autocompleteService.SearchLocations(request.Input, request.SessionToken, ct: cancellationToken);

            return OperationResult<List<AddressSuggestionDto>>.Success(result);
        }
    }
}
