using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetTags;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.TagHandlers
{
    public class GetTagsHandler : IRequestHandler<GetTagsQuery, OperationResult<List<TagResponse>>>
    {
        private readonly ITagRepository _tagRepository;
        public GetTagsHandler(
            ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        public async Task<OperationResult<List<TagResponse>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return OperationResult<List<TagResponse>>.Success(
                (await _tagRepository.GetAllAsync(cancellationToken))
                .Where(t => t.IsActive)
                .Select(t => new TagResponse
                {
                    Id = t.Id,
                    Name = t.Name,
                    IconName = t.IconUrl
                }).ToList()
            );
        }
    }
}
