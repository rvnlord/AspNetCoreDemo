using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using static AspNetCoreDemo.Startup;

namespace AspNetCoreDemo.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            var httpContext = ((AuthorizationFilterContext) context.Resource)?.HttpContext ?? HttpContextAccessor.HttpContext; // fallback to general 'HttpContext', because 'AuthorizationFilterContext' will be null if we are using the handler not in the context of specific user (in '_Layout.cshtml' View for instance)
            var loggedInAdminId =  context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var adminIdBeingEdited = httpContext.Request.Query["userId"].FirstOrDefault();

            if (context.User.IsInRole("Admin") 
                && context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")
                && adminIdBeingEdited != loggedInAdminId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    } 
}
