using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Commands;
using Renty.Application.Common;
using Renty.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Renty.Application.Handlers.UserHandlers
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, OperationResult<Unit>>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateUserProfileHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult<Unit>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            // Get current user ID from claims
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
                return OperationResult<Unit>.Fail("User not authenticated");

            // Load user with all related data for update
            var user = await _context.Users
                .Include(u => u.Languages)
                .Include(u => u.Facts)
                .FirstOrDefaultAsync(u => u.Id == currentUserId.Value, cancellationToken);

            if (user == null)
                return OperationResult<Unit>.Fail("User not found");

            // Enforce owner-only edit: current user can only edit their own profile
            if (user.Id != currentUserId.Value)
                return OperationResult<Unit>.Fail("Unauthorized: You can only edit your own profile");

            // Update user fields
            user.FirstName = request.Input.FirstName ?? user.FirstName;
            user.LastName = request.Input.LastName ?? user.LastName;
            user.AvatarUrl = request.Input.AvatarUrl ?? user.AvatarUrl;
            user.Info = request.Input.Info;
            user.HomeCityId = request.Input.HomeCityId;

            // Update languages
            user.Languages.Clear();
            if (request.Input.LanguageIds != null && request.Input.LanguageIds.Any())
            {
                var langs = await _context.Languages
                    .Where(l => request.Input.LanguageIds.Contains(l.Id))
                    .ToListAsync(cancellationToken);

                foreach (var lang in langs)
                    user.Languages.Add(lang);
            }

            // Update facts
            _context.UserFacts.RemoveRange(user.Facts);
            if (request.Input.Facts != null && request.Input.Facts.Any())
            {
                foreach (var f in request.Input.Facts)
                {
                    var fact = new Renty.Domain.Models.User.UserFact 
                    { 
                        UserId = user.Id, 
                        Type = f.Type, 
                        Value = f.Value ?? string.Empty 
                    };
                    _context.UserFacts.Add(fact);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }

        private Guid? GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User.Identity?.IsAuthenticated != true)
                return null;

            var claim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(claim, out var userId))
                return userId;

            return null;
        }
    }
}
