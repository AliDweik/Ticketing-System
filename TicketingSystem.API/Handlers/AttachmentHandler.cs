using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketingSystem.API.Requierments;
using TicketingSystem.Data.Data;

namespace TicketingSystem.API.Handlers
{
    public class AttachmentHandler : AuthorizationHandler<AttachmentRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;

        public AttachmentHandler(IHttpContextAccessor contextAccessor, ApplicationDbContext context)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AttachmentRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                     context.User.FindFirst("userId") ??
                     context.User.FindFirst("sub");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var currentUserId))
            {
                context.Fail();
                return;
            }

            // 2. Get ticket ID from route
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            var routeData = httpContext.GetRouteData();
            if (!routeData.Values.TryGetValue("attachmentId", out var attachmentIdObj) ||
                !Guid.TryParse(attachmentIdObj?.ToString(), out var attachmentId))
            {
                context.Fail();
                return;
            }

            // 3. Get ticket from database
            var attachment = await _context.TicketAttachments
                .AsNoTracking().Include(t => t.Ticket)
                .FirstOrDefaultAsync(t => t.Id == attachmentId);

            if (attachment == null)
            {
                context.Fail();
                return;
            }

            // 4. Check authorization
            if (currentUserId == attachment.Ticket.CreatedById ||
                currentUserId == attachment.Ticket.AssignedToId) // Optional admin override
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

        }
    }
}
