using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TicketingSystem.API.Requierments;
using TicketingSystem.Data.Data;

namespace TicketingSystem.API.Handlers
{
    public class UserActiveHandler : AuthorizationHandler<UserRequirement>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserActiveHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            var routeData = httpContext.GetRouteData();

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                    context.User.FindFirst("userId") ??
                    context.User.FindFirst("sub");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                context.Fail();
                return;
            }

            var user =  await _context.Users.FindAsync(userId);
            if (user.IsActive == false)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
            return;
        }
    }
}
