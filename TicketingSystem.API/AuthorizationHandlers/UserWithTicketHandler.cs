using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TicketingSystem.API.Requierments;
using TicketingSystem.Data.Data;

namespace TicketingSystem.API.Handlers
{
    public class UserWithTicketHandler : AuthorizationHandler<UserWithTicketRequirement>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserWithTicketHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserWithTicketRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            var routeData = httpContext.GetRouteData();

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                    context.User.FindFirst("userId") ??
                    context.User.FindFirst("sub");

            if (!routeData.Values.TryGetValue("ticketId", out var ticketIdObj) ||
            !Guid.TryParse(ticketIdObj.ToString(), out var ticketId))
            {
                context.Fail();
                return;
            }

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                context.Fail();
                return;
            }

            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                context.Fail();
                return;
            }

            if (ticket.CreatedById == userId)
            {
                context.Succeed(requirement);
                return;
            }

            if (ticket.AssignedToId == userId)
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();

        }
    }
}
