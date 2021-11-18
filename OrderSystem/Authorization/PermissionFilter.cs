using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace OrderSystem.Authorization
{
    public class PermissionFilter : Attribute, IAsyncAuthorizationFilter
    {
        public string[] permissions { get; set; }

        public PermissionFilter(params string[] permissions)
        {
            this.permissions = permissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new PermissionAuthorizationRequirement(permissions));
            if (!authorizationResult.Succeeded)
            {
                // authorize set  UnauthorizedResult
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
