using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
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
                context.Result = new UnauthorizedResult();
                bool isLogin = false;
                foreach (Claim claim in context.HttpContext.User.Claims)
                {
                    if (claim.Type == "Account")
                    {
                        isLogin = true;
                        break;
                    }
                }
                // authorize set  UnauthorizedResult
                if (isLogin)
                {
                    // you can direct to 401 by change this
                    // context.Result = new UnauthorizedResult();
                    context.Result = new RedirectToActionResult("Index", "Dashboard", null);
                }
                else
                {
                    context.Result = new RedirectToActionResult("SignIn", "User", null);
                }
            }
        }
    }
}
