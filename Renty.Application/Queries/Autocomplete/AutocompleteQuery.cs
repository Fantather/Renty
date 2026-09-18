using MediatR;
using Renty.Application.Common;
using Renty.Domain.ServiceModels.Places;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries.Autocomplete
{
    /// <summary>
    /// Запрос на получение списка найденных локаций для автозаполнения  
    /// </summary>
    /// <param name="Input">Текст поиска</param>
    /// <param name="SessionToken">Сессионный токен для группировки этапов запроса и выбора в поиске автозаполнения</param>
    public record AutocompleteQuery(string Input, string SessionToken) : IRequest<OperationResult<List<AddressSuggestionDto>>>;
}
