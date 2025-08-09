using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TicketingSystem.API.Requierments;
using TicketingSystem.Data.Data;

namespace TicketingSystem.API.Handlers
{
    public class UserWithoutTicketHandler : AuthorizationHandler<UserWithoutTicketRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserWithoutTicketHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserWithoutTicketRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            var routeData = httpContext.GetRouteData();

            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                    context.User.FindFirst("userId") ??
                    context.User.FindFirst("sub");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userClaimId))
            {
                context.Fail();
                return;
            }

            if(!routeData.Values.TryGetValue("userId", out var userIdObj) ||
            !Guid.TryParse(userIdObj.ToString(), out var userId))
            {
                context.Fail();
                return;
            }

            if(userClaimId == userId)
            {

                context.Succeed(requirement);
                return;
            }

            context.Fail();

        }
    }
}
