using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Data.Helpers
{
    public class PolicyOrRoleAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private readonly string _policy;
        private readonly string _role;

        public PolicyOrRoleAttribute(string policy, string role)
        {
            _policy = policy;
            _role = role;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var user = context.HttpContext.User;

            // Check policy
            var policyResult = await authService.AuthorizeAsync(user, _policy);

            // Check role
            var roleResult = user.IsInRole(_role);

            if (!policyResult.Succeeded && !roleResult)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
